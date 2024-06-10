<script setup>
import { ref, computed, onMounted, inject } from "vue";
import { useUserInfoStore } from "../../stores/user.js";
import router from "../../router/router.js";
import { ElMessage } from "element-plus";

const filterTableData = computed(() =>
    tableData.value.filter(
        (data) =>
            !search.value ||
            data.name.toLowerCase().includes(search.value.toLowerCase())
    )
);

const handleDelete = async (index, row) => {
    const data = await axios
        .post("tour/order/update", { id: row.id, status: "3" })
        .then((row) => row.data);
    if (data.code !== 200) {
        ElMessage({ message: data.value, type: "error" });
    } else {
        ElMessage({ message: "取消订单成功", type: "success" });
        meInfo.value.balance += row.price;
        row.status = "3";
    }
    console.log(row);
};
const axios = inject("$axios");

const meInfo = ref({});

onMounted(async () => {
    user.page = "user";
    if (user.id === null) {
        await router.replace("/login");
        return;
    }
    tableData.value = await axios
        .post("tour/order/query", { userId: user.id })
        .then((e) => e.data);
    let res = await axios.get("tour/user/info").then((e) => e.data.value);
    meInfo.value = res;
    modifyMeInfo.value = JSON.parse(JSON.stringify(res));
});

const user = useUserInfoStore();
const modifyMeInfo = ref({});
const tableData = ref([]);

const cancelOrder = ref(false);
const dialogVisible = ref(false);
const chargeDialogVisible = ref(false)
const type = ref(0);
const title = ref("编辑");

const chargeInfo = ref({"type":"微信","count":""})

const submitForm = async () => {
    // 改信息
    if (type.value === 0) {
        let data = await axios
            .post("tour/user/modifyByUser", modifyMeInfo.value)
            .then((e) => e.data);
        if (data.code !== 200) {
            ElMessage({ message: data.value, type: "error" });
        } else {
            ElMessage({ message: data.value, type: "success" });
            dialogVisible.value = false;
            router.go(0);
        }
    }
    if (type.value === 1) {
        if (modifyMeInfo.value.newPasswd !== modifyMeInfo.value.newPasswd2) {
            ElMessage({ message: "两次密码不一致", type: "error" });
            return;
        }
        let data = await axios
            .post("tour/user/modifyPasswordByUser", {
                passwd: modifyMeInfo.value.passwd,
                newPasswd: modifyMeInfo.value.newPasswd,
            })
            .then((e) => e.data);
        if (data.code !== 200) {
            ElMessage({ message: data.value, type: "error" });
        } else {
            ElMessage({ message: data.value, type: "success" });
            dialogVisible.value = false;
        }
    }
};

const charge = async (e) => {
    let data = await axios
        .post("tour/user/charge",chargeInfo.value)
        .then((e) => e.data);
    chargeDialogVisible.value = false
    tableData.value = await axios
        .post("tour/order/query", { userId: user.id })
        .then((e) => e.data);
    let res = await axios.get("tour/user/info").then((e) => e.data.value);
    meInfo.value = res;
    modifyMeInfo.value = JSON.parse(JSON.stringify(res));
    
}
const modify = (e) => {
    if (e === 0) {
        title.value = "编辑";
    } else {
        title.value = "修改密码";
        modifyMeInfo.value.passwd = "";
        modifyMeInfo.value.newPasswd = "";
        modifyMeInfo.value.newPasswd2 = "";
    }
    type.value = e;
    dialogVisible.value = true;
};

const showCharge = (e) => {
    chargeDialogVisible.value = true
}

const handleClose = async () => { };
const search = ref("");
</script>

<template>
    <el-col>
        <el-row>
            <el-button-group>
                <el-button type="primary" @click="modify(0)">编辑</el-button>
                <el-button type="primary" @click="modify(1)">重置密码</el-button>
                <el-button type="primary" @click="showCharge">充值</el-button>
            </el-button-group>
        </el-row>

        <el-row style="margin-top: 20px">
            <el-descriptions :title="meInfo.name" border>
                <el-descriptions-item label="姓名">
                    <div style="width: 300px">{{ meInfo.name }}</div>
                </el-descriptions-item>
                <el-descriptions-item label="名称">
                    <div style="width: 300px">{{ meInfo.aliasName }}</div>
                </el-descriptions-item>
                <el-descriptions-item label="余额">
                    <div style="width: 300px">{{ meInfo.balance }}</div>
                </el-descriptions-item>
                <el-descriptions-item label="性别">{{
                    meInfo.sex === "1" ? "男" : "女"
                    }}</el-descriptions-item>
                <el-descriptions-item label="出生日期">{{
                    meInfo.birthDate
                    }}</el-descriptions-item>

                <el-descriptions-item label="手机号">
                    {{ meInfo.phone }}
                </el-descriptions-item>
                <el-descriptions-item label="邮箱">
                    {{ meInfo.email }}
                </el-descriptions-item>
                <el-descriptions-item label="地址">
                    {{ meInfo.address }}
                </el-descriptions-item>
            </el-descriptions>
            <el-divider />
            <transition style="height: calc(100vh - 360px)">
                <el-table :data="filterTableData" style="width: 100%">
                    <el-table-column label="订单号" prop="id" />
                    <el-table-column label="名称" prop="name" />
                    <el-table-column label="有效日期" prop="validDate" />
                    <el-table-column label="价格" prop="price" />
                    <el-table-column label="状态" prop="status">
                        <template #default="scope">
                            {{
                            scope.row.status === "1"
                            ? "支付成功"
                            : scope.row.status === "2"
                            ? "已使用"
                            : "已取消"
                            }}
                        </template>
                    </el-table-column>
                    <el-table-column align="right">
                        <template #header>
                            <el-input v-model="search" size="small" placeholder="Type to search" />
                        </template>
                        <template #default="scope">
                            <el-button size="small" v-show="scope.row.status === '1'" type="danger"
                                @click="handleDelete(scope.$index, scope.row)">
                                取消订单
                            </el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </transition>
        </el-row>
    </el-col>
    <el-dialog v-model="chargeDialogVisible" title="充值" width="500">
        <el-form :model="modifyMeInfo" label-width="auto" style="max-width: 600px">
            <el-form-item label="充值方式">
                <el-radio-group v-model="chargeInfo.type">
                    <el-radio value="微信" border>
                        <div style="display: flex; ">
                            <svg t="1716600874639" class="icon" viewBox="0 0 1024 1024" version="1.1"
                                xmlns="http://www.w3.org/2000/svg" p-id="3615" width="25" height="25">
                                <path
                                    d="M1010.8 628c0-141.2-141.3-256.2-299.9-256.2-168 0-300.3 115.1-300.3 256.2 0 141.4 132.3 256.2 300.3 256.2 35.2 0 70.7-8.9 106-17.7l96.8 53-26.6-88.2c70.9-53.2 123.7-123.7 123.7-203.3zM618 588.8c-22.1 0-40-17.9-40-40s17.9-40 40-40 40 17.9 40 40c0 22-17.9 40-40 40z m194.3-0.3c-22.1 0-40-17.9-40-40s17.9-40 40-40 40 17.9 40 40-17.9 40-40 40z"
                                    fill="#00C800" p-id="3616"></path>
                                <path
                                    d="M366.3 106.9c-194.1 0-353.1 132.3-353.1 300.3 0 97 52.9 176.6 141.3 238.4l-35.3 106.2 123.4-61.9c44.2 8.7 79.6 17.7 123.7 17.7 11.1 0 22.1-0.5 33-1.4-6.9-23.6-10.9-48.3-10.9-74 0-154.3 132.5-279.5 300.2-279.5 11.5 0 22.8 0.8 34 2.1C692 212.6 539.9 106.9 366.3 106.9zM247.7 349.2c-26.5 0-48-21.5-48-48s21.5-48 48-48 48 21.5 48 48-21.5 48-48 48z m246.6 0c-26.5 0-48-21.5-48-48s21.5-48 48-48 48 21.5 48 48-21.5 48-48 48z"
                                    fill="#00C800" p-id="3617"></path>
                            </svg>
                        </div>
                    </el-radio>
                    <el-radio value="支付宝" border>
                        <div style="display: flex; ">
                            <svg t="1716601304830" class="icon" viewBox="0 0 1024 1024" version="1.1"
                                xmlns="http://www.w3.org/2000/svg" p-id="4599" width="25" height="25">
                                <path
                                    d="M860.16 0C950.272 0 1024 73.889684 1024 164.163368v531.509895s-32.768-4.122947-180.224-53.355789c-40.96-14.362947-96.256-34.896842-157.696-57.478737 36.864-63.595789 65.536-137.485474 86.016-215.444211h-202.752v-71.841684h247.808V256.512h-247.808V135.437474h-100.352c-18.432 0-18.432 18.458947-18.432 18.458947v104.663579H200.704v41.040842h249.856v69.793684H243.712v41.013895H645.12c-14.336 51.307789-34.816 98.519579-57.344 141.608421-129.024-43.115789-268.288-77.985684-356.352-55.403789-55.296 14.362947-92.16 38.992842-112.64 63.595789-96.256 116.978526-26.624 295.504842 176.128 295.504842 120.832 0 237.568-67.718737 327.68-178.526316C757.76 742.858105 1024 853.692632 1024 853.692632v6.144C1024 950.110316 950.272 1024 860.16 1024H163.84C73.728 1024 0 950.137263 0 859.836632V164.163368C0 73.889684 73.728 0 163.84 0h696.32zM268.126316 553.121684c93.049263-10.374737 180.062316 26.974316 283.270737 78.874948-74.886737 95.501474-165.941895 155.701895-256.970106 155.701894-157.830737 0-204.368842-126.652632-125.466947-197.200842 26.300632-22.851368 72.838737-35.301053 99.166316-37.376z"
                                    fill="#00A0EA" p-id="4600"></path>
                            </svg>
                        </div>
                    </el-radio>
                </el-radio-group>

            </el-form-item>
            <el-form-item label="充值金额">
                <el-input v-model="chargeInfo.count" />
            </el-form-item>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="charge"> 提交 </el-button>
            </div>
        </template>
    </el-dialog>
    <el-dialog v-model="dialogVisible" :title="title" width="500">
        <div v-show="type === 0">
            <el-form :model="modifyMeInfo" label-width="auto" style="max-width: 600px">
                <el-form-item label="姓名">
                    <el-input v-model="modifyMeInfo.name" />
                </el-form-item>
                <el-form-item label="名称">
                    <el-input v-model="modifyMeInfo.aliasName" />
                </el-form-item>
                <el-form-item label="性别">
                    <el-radio-group v-model="modifyMeInfo.sex">
                        <el-radio value="1">男</el-radio>
                        <el-radio value="2">女</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="出生日期" required>
                    <el-date-picker v-model="modifyMeInfo.birthDate" type="date" size="default" />
                </el-form-item>
                <el-form-item label="手机号">
                    <el-input v-model="modifyMeInfo.phone" />
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="modifyMeInfo.email" />
                </el-form-item>
                <el-form-item label="地址">
                    <el-input v-model="modifyMeInfo.address" />
                </el-form-item>
            </el-form>
        </div>
        <div v-show="type === 1">
            <el-form :model="modifyMeInfo" label-width="auto" style="max-width: 600px">
                <el-form-item label="旧密码">
                    <el-input v-model="modifyMeInfo.passwd" type="password" />
                </el-form-item>
                <el-form-item label="新密码">
                    <el-input v-model="modifyMeInfo.newPasswd" type="password" />
                </el-form-item>
                <el-form-item label="重复密码">
                    <el-input v-model="modifyMeInfo.newPasswd2" type="password" />
                </el-form-item>
            </el-form>
        </div>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="submitForm"> 提交 </el-button>
            </div>
        </template>
    </el-dialog>
    <el-dialog v-model="cancelOrder" title="取消订单" width="500" :before-close="handleClose">
        <span>确定取消订单？</span>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="cancelOrder = false">
                    确定
                </el-button>
            </div>
        </template>
    </el-dialog>
</template>

<style lang="scss" scoped></style>
