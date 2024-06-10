<script setup>
import {ref, onMounted, inject, nextTick} from "vue";
import {useUserInfoStore} from "../../stores/user.js";
import router from "../../router/router.js";
import {ElMessage} from "element-plus";

const changeIt = () => {
    isEdit.value = !isEdit.value
}
const isEdit = ref(false)
const value1 = ref(false)
const user = useUserInfoStore()
let hotCity = ref([])
const inputValue = ref('')
const inputVisible = ref(false)
const InputRef = ref()
const sayHello2 = () => {
    console.log('sayHello2');
}
const handleClose = async (tag) => {
    let index = hotCity.value.map(e => e.name).indexOf(tag)
    hotCity.value.splice(index, 1)
    let data = await axios.post("tour/properties/save", {
        name: "hotCity",
        value: hotCity.value.map(e => e.name).join(";")
    }).then(e => e.data)
    if (data.code !== 200) {
        ElMessage({message: "删除失败", type: "error"})
    } else {
        ElMessage({message: "删除成功", type: "success"})
    }
}

const showInput = () => {
    inputVisible.value = true
    nextTick(() => {
        InputRef.value.input.focus()
    })
}

const handleInputConfirm = () => {
    inputVisible.value = false
    inputValue.value = ''
}
const confirm = async () => {
    if (inputValue.value) {
        hotCity.value.push({isActive: false, name: inputValue.value})
    }
    let data = await axios.post("tour/properties/save", {
        name: "hotCity",
        value: hotCity.value.map(e => e.name).join(";")
    }).then(e => e.data)
    if (data.code !== 200) {
        ElMessage({message: "添加热门城市失败", type: "error"})
    } else {
        ElMessage({message: "添加成功", type: "success"})
    }
}
const axios = inject('$axios')

const goDetail = (e) => {
    router.push("/product/detail/" + e.id)
}
const tableData = ref({})
onMounted(async () => {
    tableData.value = await axios.post("/tour/scenery/list", {attr3: "active"}).then(e => e.data)
    for (let valueElement of tableData.value) {
        let minPriceItem = valueElement.product.reduce((min, item) => {
            return item.price < min.price ? item : min;
        }, valueElement.product[0]);
        valueElement.price = minPriceItem === undefined ? 0 : minPriceItem.price
        if (valueElement.score > 4.5) {
            valueElement.evaluate = '棒极了'
        } else if (valueElement.score > 4) {
            valueElement.evaluate = '挺好的'
        } else {
            valueElement.evaluate = '还不错'

        }
    }
    let res = await axios.get("tour/properties/query/hotCity").then(e => e.data)
    if (res.code !== 200) {
        ElMessage({message: "查询热门城市失败", type: "error"})
        return
    }
    res.value.split(";").forEach(e => hotCity.value.push({isActive: false, name: e}))
})

const tagChange = async (e) => {
    e.isActive = !e.isActive
    let hot = hotCity.value.filter(e => e.isActive).map(e => e.name).join(";")
    if (hot === '') {
        tableData.value = await axios.post("/tour/scenery/list", {attr3: "active"}).then(e => e.data)

    } else {
        tableData.value = await axios.post("/tour/scenery/list", {attr3: "active", attr1: hot}).then(e => e.data)

    }
    for (let valueElement of tableData.value) {
        let minPriceItem = valueElement.product.reduce((min, item) => {
            return item.price < min.price ? item : min;
        }, valueElement.product[0]);
        valueElement.price = minPriceItem === undefined ? 0 : minPriceItem.price
        if (valueElement.score > 4.5) {
            valueElement.evaluate = '棒极了'
        } else if (valueElement.score > 4) {
            valueElement.evaluate = '挺好的'
        } else {
            valueElement.evaluate = '还不错'
        }
    }
}
</script>

<template>
    <el-row>
        <el-col :span="22">
            <div class="fix1">
                热门城市：
                <el-check-tag v-show="!isEdit" :checked="item.isActive" type="primary"
                              @change="tagChange(item)" v-for="(item,index) in hotCity"
                              :class="index!==0?'ml10':''">
                    {{ item.name }}
                </el-check-tag>
                <el-tag v-show="isEdit" v-for="(item,index) in hotCity" :key="index" closable
                        @close="handleClose(item.name)"
                        :class="index!==0?'ml10':''">
                    {{ item.name }}
                </el-tag>
                <el-input v-show="isEdit"
                          v-if="inputVisible"
                          ref="InputRef"
                          v-model="inputValue"
                          class="ml10"
                          style="width: 79px"
                          size="small"
                          @keyup.enter="confirm"
                          @blur="handleInputConfirm"

                />
                <el-button v-show="isEdit" v-else class="button-new-tag ml10" size="small" @click="showInput">
                    + 添加
                </el-button>
            </div>
        </el-col>
        <el-col :span="2" v-show="user.isAdmin">
            编辑：
            <el-switch v-model="value1" @change="changeIt" label=" 编辑"></el-switch>
        </el-col>
    </el-row>
    <el-scrollbar style="padding-right: 15px;height:  calc(100vh - 160px);">
        <div
            class="box"
            style="
                    width: 100%;
                    display: flex;
                    flex-wrap: wrap;
                    gap: 50px;
                    justify-content: space-between;
                    ::after {
                      content: '';
                      flex: auto;
                    }
                   flex-shrink: 0;
                "
        >

            <div v-for="item in tableData">
                <el-card
                    :body-style="{ padding: '8px', height: '144px', width: '360px'}"
                    class="clickable-card"
                    @click="goDetail(item)"
                >
                    <el-row>
                        <el-col :span="8" style="align-items: center">
                            <el-image
                                style="width:110px; height: 144px"
                                :src="item.imageList!==null?item.imageList[0].url:''"
                                fit="cover"
                            />
                        </el-col>
                        <el-col :span="16">
                            <el-row style="height: 36px; align-content: center">
                                <div class="title">{{ item.name }}</div>
                                <div class="G4A" v-show="!item.isHotel">{{ item.grade }}</div>
                            </el-row>
                            <el-row
                                style="
                                            height: 27px;
                                            line-height: 27px;
                                            align-content: center;
                                            font-size: 16px;
                                            align-items: baseline;
                                            vertical-align: middle;
                                          "
                            >
                                <p style="color: #23beae; font-weight: 600; font-size: 22px">{{
                                        item.score
                                    }}</p>
                                <span style="color: #23beae">分</span>
                                <p class="ml10" style="color: #23beae; font-weight: 600">{{ item.evaluate }}</p>

                                <p class="ml10" style="color: #999">{{ item.commentCount }}条评论</p>
                            </el-row>
                            <el-row style="height: 27px; align-content: center; color: #999">
                                <p style="">{{ item.type }}</p>
                                <p class="line"></p>
                                <p>{{ item.province }} {{ item.city }}</p>
                            </el-row>
                            <el-row style="align-content: center; align-items: baseline; height: 27px">
                                <p style="color: #ff6248">￥</p>
                                <p style=" height: 28px; font-size: 22px;  font-weight: 600; color: #ff6248;">
                                    {{ item.price }}
                                </p>
                                <p>起</p>
                            </el-row>
                            <el-row style="align-content: center; align-items: baseline; height: 27px">
                                <p style="font-size: 18px; font-weight: 600">特色</p>
                                <p class="ml10" style="font-size: 18px; font-weight: 600">·</p>
                                <p class="ml10" style="font-size: 13px">{{ item.summary }}</p>
                            </el-row>
                        </el-col>
                    </el-row>
                </el-card>
            </div>
        </div>
    </el-scrollbar>

</template>

<style scoped>
.line {
    display: inline-block;
    width: 1px;
    background-color: #999;
    margin: 4px 6px;
}

p {
    margin: 0;
}

.clickable-card {
    max-width: 480px;
    cursor: pointer;
    transition: box-shadow 0.3s;
}

.clickable-card:hover {
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.fix1 {
    background: #ffffff;
    padding-bottom: 20px;
    height: 25px;
}

.G4A {
    color: #fff;
    font-weight: 700;
    background: linear-gradient(-90deg, #cf7632, #f8b891);
    font-size: 18px;
    width: max-content;
    padding: 0 6px 0 6px;
    border-radius: 3px;
    display: inline-block;
    margin-left: 6px;
    height: 28px;
    line-height: 28px;
}

.title {
    font-weight: 700;
    font-size: 24px;
    display: inline-block;
}

.ml10 {
    margin-left: 6px;
}

.box:after {
    content: "";
    width: 378px;
}
</style>
