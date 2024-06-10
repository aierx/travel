<script setup>
import CreateScenery from "./SceneryCreate.vue";
import {ref, computed, onMounted, inject} from "vue";
import {useUserInfoStore} from "../../stores/user.js";
import HotelCreate from "./HotelCreate.vue";
import {ElMessage} from "element-plus";
import router from "../../router/router.js";

const isShowTable = ref(true)
const user = useUserInfoStore()
onMounted(() => {
    user.secondPage = "hotel"
})
const search = ref('')
const filterTableData = computed(() =>
    tableData.value.filter(
        (data) => !search.value || data.name.toLowerCase().includes(search.value.toLowerCase())
    )
)

const onBackFun = () => {
    isShowTable.value = true
    router.go(0)
}
const form = ref([])

const handleEdit = (index, row) => {
    form.value = row
    isShowTable.value = !isShowTable
}
const handleDelete = async (index, row) => {
    let res = await axios.post("/tour/scenery/delete", [row.id]).then(e => e.data)
    if (res.code !== 200) {
        ElMessage({message: res.value, type: "error"})
    } else {
        ElMessage({message: "删除成功", type: "success"})
        router.go(0)
    }
}


const batchDelete = async () => {
    let a = selectRef.value.map(e => e.id)
    if (a.length === 0) {
        ElMessage({message: "请选择", type: "error"})
        return
    }
    let res = await axios.post("/tour/scenery/delete", a).then(e => e.data)
    if (res.code !== 200) {
        ElMessage({message: res.value, type: "error"})
    } else {
        ElMessage({message: "删除成功", type: "success"})
        router.go(0)
    }
}
const add = () => {
    isShowTable.value = !isShowTable
    form.value = undefined
}
const axios = inject("$axios")
onMounted(async () => {
    console.log("CreateScenery onmount")
    tableData.value = await axios.post("/tour/scenery/list", {attr2: "hotel"}).then(e => e.data)
})
const tableData = ref([])

const handleStatusChange = (e1) => {
    axios.post("/tour/scenery/reversalStatus", {id: e1})
}
const selectRef = ref([])
const aaa = (e) => {
    console.log(e)
    selectRef.value = e
}
</script>

<template>
    <div v-if="isShowTable">
        <el-row>
            <el-button-group>
                <el-button type="primary" @click="add">新增</el-button>
                <el-button type="danger" @click="batchDelete">批量删除</el-button>
            </el-button-group>
        </el-row>
        <el-row style="margin-top: 20px">
            <el-table :data="filterTableData" style="width: 100%" @selection-change="aaa" height="calc(100vh - 180px)">
                <el-table-column type="selection" width="55"/>
                <el-table-column label="名称" prop="name" width="180"/>
                <el-table-column label="类型" prop="type" width="100"/>
                <el-table-column label="手机号" prop="phone" width="140"/>
                <el-table-column label="评分" prop="score" width="80"/>
                <el-table-column label="状态" prop="status" width="180">
                    <template #default="scope">
                        <el-switch @change="handleStatusChange(scope.row.id)" v-model="scope.row.status"/>
                    </template>
                </el-table-column>
                <el-table-column label="地址" prop="address"/>
                <el-table-column align="right" width="200">
                    <template #header>
                        <el-input v-model="search" size="small" placeholder="Type to search"/>
                    </template>
                    <template #default="scope">
                        <el-button size="small" @click="handleEdit(scope.$index, scope.row)">
                            编辑
                        </el-button>
                        <el-button
                            size="small"
                            type="danger"
                            @click="handleDelete(scope.$index, scope.row)">
                            删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </el-row>
    </div>
    <el-row v-if="!isShowTable">
        <hotel-create @onBack="onBackFun" :form="form"/>
    </el-row>
</template>

<style>
</style>


