<template>
    <el-col>
        <el-row>
            <el-button-group>
                <el-button type="primary" @click="modify(2)">新增</el-button>
            </el-button-group>
        </el-row>
        <el-row>
            <el-table :data="filterTableData" style="width: 100%;margin-top: 20px" height="calc(100vh - 180px)">
                <el-table-column label="用户号" prop="userNo"/>
                <el-table-column label="姓名" prop="name"/>
                <el-table-column label="名称" prop="aliasName"/>
                <el-table-column label="性别">
                    <template #default="scope">
                        {{ scope.row.sex === '1' ? '男' : '女' }}
                    </template>
                </el-table-column>
                <el-table-column label="手机号" prop="phone" width="140"/>
                <el-table-column label="邮箱" prop="email" width="220"/>
                <el-table-column label="地址" prop="address" width="220"/>
                <el-table-column label="余额" prop="balance"/>
                <el-table-column label="管理员">
                    <template #default="scope">
                        <el-switch @change="handleStatusChange(scope.row.id)" v-model="scope.row.isAdmin" size="small"/>
                    </template>
                </el-table-column>
                <el-table-column align="right" width="160px" fixed="right">
                    <template #header>
                        <el-input v-model="search" size="small" placeholder="Type to search"/>
                    </template>
                    <template #default="scope">
                        <el-button size="small" @click="modify(0,scope.row)">
                            编辑
                        </el-button>
                        <el-button size="small" @click="modify(1,scope.row)">
                            修改密码
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </el-row>
    </el-col>


    <el-dialog
        v-model="dialogVisible"
        :title="title"
        width="500"
    >
        <div v-show="type===0">
            <el-form :model="userInfo" label-width="auto" style="max-width: 600px"
                     :rules="{balance:[{required:true,message:'金额不对',pattern:/^[0-9]+([.]{1}[0-9]{1,2})?$/}]}">
                <el-form-item label="姓名">
                    <el-input v-model="userInfo.name"/>
                </el-form-item>
                <el-form-item label="用户名">
                    <el-input v-model="userInfo.aliasName"/>
                </el-form-item>
                <el-form-item label="性别">
                    <el-radio-group v-model="userInfo.sex">
                        <el-radio value="1">男</el-radio>
                        <el-radio value="2">女</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item
                    label="出生日期"
                    required
                >
                    <el-date-picker
                        v-model="userInfo.birthDate"
                        type="date"
                        size="default"
                    />
                </el-form-item>
                <el-form-item label="手机号">
                    <el-input v-model="userInfo.phone"/>
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="userInfo.email"/>
                </el-form-item>
                <el-form-item label="地址">
                    <el-input v-model="userInfo.address"/>
                </el-form-item>
                <el-form-item label="余额" prop="balance">
                    <el-input v-model="userInfo.balance" type="text"/>
                </el-form-item>
                <el-form-item label="密码" v-show="title==='新增'">
                    <el-input v-model="userInfo.passwd" type="password"/>
                </el-form-item>
                <el-form-item label="管理员">
                    <el-switch v-model="userInfo.isAdmin"/>
                </el-form-item>
            </el-form>
        </div>

        <div v-show="type===1">
            <el-form :model="userInfo" label-width="auto" style="max-width: 600px">
                <el-form-item label="新密码">
                    <el-input v-model="userInfo.newPasswd" type="password"/>
                </el-form-item>
            </el-form>
        </div>
        <template #footer>
            <div class="dialog-footer">
                <el-button type="primary" @click="submit(2)">
                    提交
                </el-button>
            </div>
        </template>
    </el-dialog>
</template>

<script setup>
import {ref, computed, onMounted, inject, getCurrentInstance} from "vue";
import {ElMessage} from 'element-plus'
import router from "../../router/router.js";
import {useUserInfoStore} from "../../stores/user.js";

const activeIndex = ref("1")

const dialogVisible = ref(false)
const type = ref(0)
const title = ref("新增")

const userInfo = ref({})
const user = useUserInfoStore()
const axios = inject("$axios")

onMounted(() => {
    user.secondPage = "user"
})


function formatDate(date) {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
}

const submit = async () => {
    dialogVisible.value = false
    if (type.value === 1) {
        let res = await axios.post("/tour/user/modifyPassword", {
            id: userInfo.value.id,
            newPasswd: userInfo.value.newPasswd
        }).then(e => e.data)
        if (res.code !== 200) {
            ElMessage({message: res.value, type: "error"})
        } else {
            ElMessage({message: "预定成功", type: "success"})
        }
    } else {
        userInfo.value.birthDate = typeof (userInfo.value.birthDate) === "string" ? userInfo.value.birthDate : formatDate(userInfo.value.birthDate)
        let res = await axios.post("/tour/user/save", userInfo.value).then(e => e.data)
        if (res.code !== 200) {
            ElMessage({message: res.value, type: "error"})
        } else {
            ElMessage({message: "预定成功", type: "success"})
        }
    }
    router.go(0)
}
const handleStatusChange = async (row) => {
    console.log(row)
    await axios.post("/tour/user/reversalAdmin", {id: row})

}
onMounted(async () => {
    console.log("UserManager Onmount")
    dialogVisible.value = false
    tableData.value = await axios.post("/tour/user/list").then(e => e.data)

})
const modify = (e, f) => {
    type.value = 0
    if (e === 2) {
        title.value = "新增"
        userInfo.value = {balance: 0}
    } else if (e === 1) {
        type.value = 1
        title.value = "修改密码"
        userInfo.value = JSON.parse(JSON.stringify(f))
    } else {
        title.value = "编辑"
        userInfo.value = JSON.parse(JSON.stringify(f))
    }
    dialogVisible.value = true
}


const search = ref('')
const filterTableData = computed(() =>
    tableData.value.filter(
        (data) =>
            !search.value || data.name.toLowerCase().includes(search.value.toLowerCase())
    )
)

function select(e) {
    activeIndex.value = e
}

const tableData = ref([])
</script>

<style lang="scss" scoped>

</style>