<script setup>
import {ref, onMounted, inject} from "vue";
import router from "../../router/router.js";
import {useUserInfoStore} from "../../stores/user.js";
import {formatDate, daysOfTwoDate} from "../../utils.js";
import {ElMessage} from "element-plus";
const reserveDate = ref([]);
const price = ref(0);
let sceneryId = "";
const axios = inject("$axios");
const activeIndex = ref("0");
const dataInfo = ref({});
const comment = ref([]);
const html = ref("");
const textarea = ref("");
const user = useUserInfoStore();
const onBack = () => {
    router.go(-1);
};
const handleSelect = (e) => {
    activeIndex.value = e;
};
const f = ref([
    {isActive: false, name: "好评"},
    {isActive: false, name: "差评"},
]);
const handleChange = () => {
    if (reserveDate.value.length === 0) {
        price.value = 0;
    } else {
        let days = daysOfTwoDate(reserveDate.value[0], reserveDate.value[1]);
        let amountPrice = 0;
        for (let el of dataInfo.value.product) {
            amountPrice += el.payCount * days * el.price;
        }
        price.value = amountPrice;
    }
};
onMounted(async () => {
    sceneryId = router.currentRoute.value.params.id;
    if (sceneryId === undefined) return;
    dataInfo.value = await axios
        .get("tour/scenery/" + sceneryId)
        .then((e) => e.data);
    html.value = JSON.parse(dataInfo.value.html)
        ? JSON.parse(dataInfo.value.html).richText
        : "";
    for (let productElement of dataInfo.value.product) {
        productElement.payCount = 0;
    }
    // 评论
    comment.value = await axios.get("tour/comment/" + sceneryId).then((e) => {
        e.data.forEach((e1) => {
            if (e1.filter !== null && e1.filter !== '') {
                e1.title = e1.userName + " - " + e1.filter;
            } else {
                e1.title = e1.userName;
            }
        });
        return e.data;
    });
    // 攻略
    strategyInfo.value = await axios
        .get("tour/strategy/" + sceneryId)
        .then((e) => {
            e.data.value.forEach((e1) => {
                e1.html = JSON.parse(e1.html).richText;
            });
            return e.data.value;
        });
    // 查询我的点赞
    let like = await axios.get("tour/strategy/like/" + sceneryId).then(e => {return e.data.value})
    strategyInfo.value.forEach(e=>{
        e.like = false
        for (let l of like) {
            if (e.id===l.strategyId){
                e.like = true
            }
        }
    })
});
const strategyInfo = ref([]);
const filter = ref("好评");
const submitComment = async () => {
    let data = {
        userId: user.id,
        userName: user.name,
        sceneryId: sceneryId,
        comment: textarea.value,
        filter: filter.value,
        title: user.name + " - " + filter.value,
    };
    comment.value.push(data);
    await axios.post("tour/comment/save", data).then((e) => e.data);
};
const submitPay = async () => {
    if (price.value === 0 || reserveDate.value.length === 0) {
        ElMessage({
            message: "请选择商品和日期",
            type: "error",
        });
        return;
    }
    let name = dataInfo.value.product
        .filter((it) => it.payCount !== 0)
        .map((it) => it.name + ":" + it.payCount)
        .join(";");
    let count = 0;
    dataInfo.value.product.forEach((e) => {
        count += e.payCount;
    });
    let validDate =
        formatDate(reserveDate.value[0]) + " ~ " + formatDate(reserveDate.value[1]);

    let data = {
        userId: user.id,
        productId: sceneryId,
        name: name,
        price: price.value,
        validDate: validDate,
        status: "1",
        count: count,
        type: dataInfo.value.isHotel ? "门票" : "酒店",
        aliasName: user.aliasName,
    };
    let res = await axios.post("tour/order/pay", data).then((e) => e.data);
    if (res.code !== 200) {
        ElMessage({message: res.value, type: "error"});
    } else {
        ElMessage({message: "预定成功", type: "success"});
    }
};
const tagChange = async (e) => {
    e.isActive = !e.isActive;

    let query = f.value
        .filter((e) => e.isActive)
        .map((e) => e.name)
        .join(",");
    comment.value = await axios
        .get("tour/comment/" + sceneryId, {params: {filter: query}})
        .then((e) => {
            e.data.forEach((e1) => {
                if (e1.filter !== null && e1.filter !== '') {

                    e1.title = e1.userName + " - " + e1.filter;
                } else {
                    e1.title = e1.userName;
                }
            });
            return e.data;
        });
};
const likeBtn = async (b)=>{
    if (b.like){
        b.like = false
        b.likeCount --
    }else {
        b.like = true
        b.likeCount ++
    }
    axios.post("tour/strategy/like",{
        userId:user.id,
        sceneryId:sceneryId,
        strategyId:b.id
    })
}
const formJson = ref({
    widgetList: [
        {
            key: 34704,
            type: "rich-editor",
            icon: "rich-editor-field",
            formItemFlag: true,
            options: {
                name: "richText",
                label: "详情",
                labelAlign: "label-center-align",
                labelHidden: true,
                columnWidth: "200px",
                contentHeight: "400px",
                disabled: false,
                hidden: false,
                required: false,
                labelIconPosition: "rear",
                showWordLimit: false,
            },
            id: "richText",
        },
    ],
    formConfig: {
        modelName: "formData",
        refName: "vForm",
        rulesName: "rules",
        labelWidth: 80,
        labelPosition: "top",
        labelAlign: "label-right-align",
        layoutType: "PC",
        jsonVersion: 3,
    },
});
const isWrite = ref(false);
const formData = ref({});
const vFormRef = ref(null);

const submitStrategy = async (e) => {
    let html = await vFormRef.value.getFormData().then((res) => {
        return JSON.stringify(res);
    });
    let data = {
        userId: user.id,
        userName: user.name,
        sceneryId: sceneryId,
        html: html,
    };
    let res = await axios.post("/tour/strategy/submit", data).then((e) => e.data);
    if (res.code !== 200) {
        ElMessage({message: res.value, type: "error"});
    } else {
        ElMessage({message: "成功", type: "success"});
        router.go(0);
    }
};
</script>
<template>
    <el-page-header
        @back="onBack"
        class="page-header-container"
        style="border-bottom: 1px solid #dcdfe6; width: 100%; padding-bottom: 10px"
    >
        <template #content>
            <div class="flex items-center">
                <span class="text-large font-600 mr-3">详情页</span>
            </div>
        </template>
    </el-page-header>
    <el-scrollbar style="height: calc(100vh - 150px)">
        <div>
            <el-row style="margin: 20px 0 0 0; height: 460px" :gutter="20">
                <el-col :span="12" style="align-content: center">
                    <el-carousel indicator-position="outside">
                        <el-carousel-item
                            v-for="item in dataInfo.imageList"
                            :key="item"
                            style="height: 380px"
                        >
                            <el-image :src="item.url"/>
                        </el-carousel-item>
                    </el-carousel>
                </el-col>
                <el-col :span="12">
                    <el-row>
                        <el-descriptions
                            :title="dataInfo.name"
                            class="m2"
                            border
                            style="width: 100%"
                        >
                            <el-descriptions-item label="类型">{{
                                    dataInfo.type
                                }}
                            </el-descriptions-item>
                            <el-descriptions-item label="评级">{{
                                    dataInfo.grade
                                }}
                            </el-descriptions-item>
                            <el-descriptions-item label="联系方式">{{
                                    dataInfo.phone
                                }}
                            </el-descriptions-item>
                            <el-descriptions-item label="地区">{{
                                    dataInfo.address
                                }}
                            </el-descriptions-item>
                            <el-descriptions-item label="特色">{{
                                    dataInfo.summary
                                }}
                            </el-descriptions-item>
                        </el-descriptions>
                    </el-row>
                    <el-row class="m2">
                        <el-text class="mx-1" size="large">预定日期：</el-text>
                        <div>
                            <el-date-picker
                                v-model="reserveDate"
                                type="daterange"
                                range-separator="To"
                                start-placeholder="Start date"
                                end-placeholder="End date"
                                @change="handleChange"
                            />
                        </div>
                    </el-row>
                    <el-row class="m2" style="height: 180px">
                        <el-table
                            :data="dataInfo.product"
                            style="width: 100%"
                            :max-height="180"
                        >
                            <el-table-column prop="name" label="名称"/>
                            <el-table-column prop="price" label="单价" width="120"/>
                            <el-table-column prop="count" label="数量" width="180">
                                <template #default="scope">
                                    <el-input-number
                                        de
                                        v-model="scope.row.payCount"
                                        @change="handleChange(scope.row)"
                                    />
                                </template>
                            </el-table-column>
                        </el-table>
                    </el-row>
                    <el-row
                        class="m2"
                        style="display: flex; justify-content: space-between"
                    >
                        <el-text class="mx-1" size="large">总金额：{{ price }}元</el-text>
                        <el-button
                            type="primary"
                            @click="submitPay"
                            :disabled="!user.isLogin"
                        >立即预定
                        </el-button
                        >
                    </el-row>
                </el-col>
            </el-row>
            <el-row>
                <el-menu
                    :default-active="activeIndex"
                    class="el-menu-demo"
                    mode="horizontal"
                    @select="handleSelect"
                    style="width: 100%; margin-right: 10px"
                >
                    <el-menu-item index="0">详情</el-menu-item>
                    <el-menu-item index="1">评论</el-menu-item>
                    <el-menu-item index="2">攻略共享</el-menu-item>
                </el-menu>
            </el-row>
            <div
                v-show="activeIndex === '0'"
                class="container"
                style="margin-right: 10px"
            >
                <el-row>
                    <p v-html="html"></p>
                </el-row>
            </div>
            <div
                v-show="activeIndex === '1'"
                class="m1"
                style="margin-right: 10px; align-content: center"
            >
                <el-row>
                    <el-input
                        v-model="textarea"
                        style="width: 100%"
                        :rows="3"
                        type="textarea"
                    />
                </el-row>
                <el-row class="m2" style="display: flex; align-items: center">
                    <el-col :span="6"></el-col>
                    <el-col :span="12"></el-col>
                    <el-col :span="4">
                        <el-radio-group v-model="filter" class="ml-4">
                            <el-radio value="好评" size="large" border>好评</el-radio>
                            <el-radio value="差评" size="large" border>差评</el-radio>
                        </el-radio-group>
                    </el-col>
                    <el-col
                        :span="2"
                        style="display: flex; flex-direction: column-reverse"
                    >
                        <el-button
                            type="primary"
                            :disabled="!user.isLogin"
                            @click="submitComment"
                        >提交评论
                        </el-button
                        >
                    </el-col>
                </el-row>
                <el-row style="margin-top: 10px; margin-bottom: 10px">
                    <el-check-tag
                        :checked="item.isActive"
                        type="primary"
                        @change="tagChange(item)"
                        v-for="(item, index) in f"
                        :class="index !== 0 ? 'ml10' : ''"
                    >
                        {{ item.name }}
                    </el-check-tag>
                </el-row>
                <el-row
                    v-for="it in comment"
                    style="border-bottom: 1px solid #dcdfe6; margin-bottom: 20px"
                >
                    <el-descriptions :title="it.title">
                        <el-descriptions-item label="">{{
                                it.comment
                            }}
                        </el-descriptions-item>
                    </el-descriptions>
                </el-row>
            </div>
            <div v-show="activeIndex === '2'" class="m1"  style="display: flex; justify-content: center">
                <div style="width: 1148px">
                    <el-row justify="center" style="margin-top: 8px">
                        <el-button v-show="!isWrite" @click="isWrite = !isWrite"
                        >写攻略
                        </el-button
                        >
                        <el-button v-show="isWrite" @click="isWrite = !isWrite"
                        >查看攻略
                        </el-button
                        >
                    </el-row>
                    <el-row v-show="isWrite">
                        <v-form-render
                            :form-json="formJson"
                            :form-data="formData"
                            ref="vFormRef"
                            style="width: 100%"
                        />
                    </el-row>
                    <el-row v-show="isWrite" justify="end">
                        <el-col
                            :span="2"
                            style="display: flex; flex-direction: column-reverse"
                        >
                            <el-button @click="submitStrategy">提交</el-button>
                        </el-col>
                    </el-row>
                    <el-row v-show="!isWrite">
                        <el-card
                            style="width: 1148px; margin-top: 20px"
                            v-for="o in strategyInfo"
                        >
                            <p v-html="o.html"></p>
                            <template #footer>
                                <div style="display: flex;justify-content: space-between;">
                                    <el-text style="margin-right:16px;">用户：{{ o.userName }}</el-text>
                                    <el-check-tag :checked="o.like" type="primary"    @change="likeBtn(o)"> 点赞 {{ o.likeCount }}</el-check-tag>
                                </div>
                            </template>
                        </el-card>
                    </el-row>
                </div>
            </div>
        </div>
    </el-scrollbar>
</template>
<style scoped>
::v-deep img {
    max-width: 100%;
}
.el-carousel__item h3 {
    color: #475669;
    opacity: 0.75;
    line-height: 200px;
    margin: 0;
    text-align: center;
}
.page-header-container {
    position: sticky;
    top: 0;
    /* 页面顶部 */
    z-index: 1000;
    /* 确保位于其他内容之上 */
    background: white;
}
.m2 {
    margin-top: 20px;
}
.m1 {
    margin-top: 10px;
}

.ml10 {
    margin-left: 6px;
}
</style>
