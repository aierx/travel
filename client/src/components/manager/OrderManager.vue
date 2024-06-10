<script setup>
import {ref, computed, reactive, onMounted, inject} from "vue";
import ScreneryManager from "./SceneryManager.vue";
import {useUserInfoStore} from "../../stores/user.js";
import {ElMessage} from "element-plus";

const activeIndex = ref("1")

const search = ref('')
const filterTableData = computed(() =>
    tableData.value.filter(
        (data) =>
            !search.value ||
            data.name.toLowerCase().includes(search.value.toLowerCase())
    )
)

function select(e) {
    activeIndex.value = e
}

const handleUse = async (row) => {
    const data = await axios.post("tour/order/update", {id: row.id, status: "2"}).then(e => e.data)
    row.status = '2'
    ElMessage({
        message: 'success',
        type: 'success',
    })
}

const handleDelete = async (row) => {
    const data = await axios.post("tour/order/update", {id: row.id, status: "3"}).then(row => row.data)
    row.status = '3'
    ElMessage({
        message: 'success',
        type: 'success',
    })

}
const user =useUserInfoStore()
const axios = inject("$axios")
onMounted(async () => {
    user.secondPage = "order"
    tableData.value = await axios.post("tour/order/query", {}).then(e => e.data);

    console.log(tableData.value)
})

const tableData = ref([])
</script>

<template>
    <el-table :data="filterTableData" style="width: 100%;height: calc(100vh - 100px)">
        <el-table-column label="订单号" prop="id"/>
        <el-table-column label="商品名" prop="name"/>
        <el-table-column label="类型" prop="type"/>
        <el-table-column label="用户名" prop="aliasName"/>
        <el-table-column label="价格" prop="price"/>
        <el-table-column label="数量" prop="count"/>
        <el-table-column label="状态">
            <template #default="scope">
                {{ scope.row.status === '1' ? "支付成功" : scope.row.status === '2' ? '已使用' : '已取消' }}
            </template>
        </el-table-column>
        <el-table-column align="right">
            <template #header>
                <el-input v-model="search" size="small" placeholder="Type to search"/>
            </template>
            <template #default="scope">
                <el-button
                    size="small"
                    type="danger"
                    @click="handleDelete(scope.row)"
                    v-show="scope.row.status==='1'"
                >
                    取消
                </el-button>
                <el-button
                    size="small"
                    type="primary"
                    @click="handleUse( scope.row)"
                    v-show="scope.row.status==='1'"
                >
                    使用
                </el-button>
            </template>
        </el-table-column>
    </el-table>
</template>

<style scoped>

</style>