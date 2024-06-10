<script setup>
import {reactive, ref, onMounted, inject} from "vue";
import {Plus} from "@element-plus/icons-vue";
import {ElMessage} from "element-plus";
import router from "../../router/router.js";

const form = ref({product: []});
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
                contentHeight: "800px",
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
})
const axios = inject("$axios")
const formData = reactive({});
const vFormRef = ref(null);

const emit = defineEmits(["onBack"])

function onBack() {
    emit("onBack")
}

const submit = async () => {
    let html = await vFormRef.value.getFormData().then(res => {
        return JSON.stringify(res)
    })
    form.value.html = html
    let res = await axios.post("/tour/scenery/save", form.value).then(e => e.data)
    if (res.code !== 200) {
        ElMessage({message: res.value, type: "error"})
    } else {
        ElMessage({message: "成功", type: "success"})
        router.go(0)
    }
}


const data = defineProps(["form"])
const uploadUrl = axios.defaults.baseURL + "/file/upload"
onMounted(() => {
    console.log("SceneryCreate mount")
    if (data.form === undefined) {
        return
    }
    form.value = data.form
    vFormRef.value.setFormData(JSON.parse(form.value.html))
})

const dialogImageUrl = ref("");
const dialogVisible = ref(false);

const handlePictureCardPreview = (uploadFile) => {
    dialogImageUrl.value = uploadFile.url;
    dialogVisible.value = true;
};

const handleSuccess = (response) => {
    let img = form.value.imageList.pop()
    img = {name: img.name, url: response.value}
    form.value.imageList.push(img)
}
// 行index
const rowIndex = ref("")

// 列index
const columnIndex = ref("")

// 给表格单元格数据添加列下标
const tabCellClassName = ({column, columnIndex}) => {
    column.index = columnIndex + 1;
}
// 表格行class-name
const tabRowClassName = ({row, rowIndex}) => {
    row.index = rowIndex + 1;
}
// 点击表格单元格编辑
const cellClick = (row, column) => {
    rowIndex.value = row.index;
    columnIndex.value = column.index;
}
// 表格input失去焦点
const hideInput = () => {
    rowIndex.value = null;
    columnIndex.value = null;
}
</script>

<template>
    <el-col>
        <el-row>
            <el-page-header @back="onBack" style="border-bottom: 1px solid #dcdfe6;width: 100%;padding-bottom: 10px">
                <template #content>
                    <div class="flex items-center">
                        <span class="text-large font-600 mr-3"> 添加景点 </span>
                    </div>
                </template>
                <template #extra>
                    <div style="margin-right: 20px">
                        <el-button @click="submit" type="primary">提交</el-button>
                    </div>
                </template>
            </el-page-header>
        </el-row>
        <el-row style="width: 100%">
            <el-scrollbar style="height: calc(100vh - 180px);padding: 15px">
                <el-form
                    :model="form"
                    label-width="auto"
                    margin: auto
                    style="max-width: 1080px; margin: auto;  padding-top: 40px"
                >
                    <el-form-item label="名称">
                        <el-input v-model="form.name"/>
                    </el-form-item>
                    <el-form-item label="类型">
                        <el-input v-model="form.type"/>
                    </el-form-item>
                    <el-form-item label="简介">
                        <el-input v-model="form.summary"/>
                    </el-form-item>
                    <el-form-item label="评分">
                        <el-input v-model="form.score"/>
                    </el-form-item>
                    <el-form-item label="等级">
                        <el-select v-model="form.grade" placeholder="请选择">
                            <el-option label="1A" value="1A"/>
                            <el-option label="2A" value="2A"/>
                            <el-option label="3A" value="3A"/>
                            <el-option label="4A" value="4A"/>
                            <el-option label="5A" value="5A"/>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="收费" prop="resource">
                        <el-row>
                            <el-radio-group v-model="form.isCharge">
                                <el-radio value="1">是</el-radio>
                                <el-radio value="2">否</el-radio>
                            </el-radio-group>
                        </el-row>
                        <el-table
                            v-show="form.isCharge==='1'"
                            :data="form.product"
                            :cell-class-name="tabCellClassName"
                            :row-class-name="tabRowClassName"
                            @cell-click="cellClick">
                            <el-table-column label="名称">
                                <template #default="scope">
                                    <el-input
                                        v-if="rowIndex === scope.row.index && columnIndex === scope.column.index"
                                        v-model="scope.row.name"
                                        @blur="hideInput"></el-input>

                                    <span v-else>{{ scope.row.name }}</span>
                                </template>
                            </el-table-column>

                            <el-table-column label="单价">
                                <template #default="scope">
                                    <el-input
                                        v-if="rowIndex === scope.row.index && columnIndex === scope.column.index"
                                        v-model="scope.row.price"
                                        @blur="hideInput"></el-input>

                                    <span v-else>{{ scope.row.price }}</span>
                                </template>
                            </el-table-column>
                            <el-table-column align="right">
                                <template #header>
                                    <el-button @click="form.product.push({})">添加</el-button>
                                </template>
                                <template #default="scope">
                                    <el-button
                                        size="small"
                                        type="danger"
                                        @click="form.product.splice(row, 1)">
                                        删除
                                    </el-button>
                                </template>
                            </el-table-column>
                        </el-table>
                    </el-form-item>
                    <el-form-item label="省-市-区">
                        <el-row :gutter="20">
                            <el-col :span="8">
                                <el-input v-model="form.province"/>
                            </el-col>
                            <el-col :span="8" class="text-center">
                                <el-input v-model="form.city"/>
                            </el-col>
                            <el-col :span="8">
                                <el-input v-model="form.area"/>
                            </el-col>
                        </el-row>
                    </el-form-item>
                    <el-form-item label="详细地址">
                        <el-input v-model="form.address"/>
                    </el-form-item>
                    <el-form-item label="联系方式">
                        <el-input v-model="form.phone"/>
                    </el-form-item>
                    <el-form-item label="图片">
                        <el-upload
                            v-model:file-list="form.imageList"
                            :action="uploadUrl"
                            list-type="picture-card"
                            :on-preview="handlePictureCardPreview"
                            :on-success="handleSuccess"
                        >
                            <el-icon>
                                <Plus/>
                            </el-icon>
                        </el-upload>
                        <el-dialog v-model="dialogVisible" style="align-content: center;text-align: center">
                            <el-image style="width: 1000px; height: 800px" :src="dialogImageUrl" :fit="contain"/>
                        </el-dialog>
                    </el-form-item>
                    <v-form-render
                        :form-json="formJson"
                        :form-data="formData"
                        ref="vFormRef"
                    />
                </el-form>
            </el-scrollbar>

        </el-row>
    </el-col>


</template>
