<template>
    <div class="cesiumTabRoot">
        <el-table
            :data="tabData"
            :cell-class-name="tabCellClassName"
            :row-class-name="tabRowClassName"
            @cell-click="cellClick">
            <el-table-column type="index" label="序号" width="50"></el-table-column>
            <el-table-column label="姓名">
                <template #default="scope">
                    <el-input
                        v-if="rowIndex === scope.row.index && columnIndex === scope.column.index"
                        v-model="scope.row.name"
                        @blur="hideInput"></el-input>

                    <span v-else>{{ scope.row.name }}</span>
                </template>
            </el-table-column>

            <el-table-column label="年龄">
                <template #default="scope">
                    <el-input
                        v-if="rowIndex === scope.row.index && columnIndex === scope.column.index"
                        v-model="scope.row.age"
                        @blur="hideInput"></el-input>

                    <span v-else>{{ scope.row.age }}</span>
                </template>
            </el-table-column>

            <el-table-column label="性别">
                <template #default="scope">
                    <el-input
                        v-if="rowIndex === scope.row.index && columnIndex === scope.column.index"
                        v-model="scope.row.sex"
                        @blur="hideInput"></el-input>
                    <span v-else>{{ scope.row.sex }}</span>
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>

<script setup>

import {ref} from "vue";

// 表格数据
const tabData = ref([
    {
        name: "张三",
        age: 18,
        sex: "男"
    },
    {
        name: "李四",
        age: 19,
        sex: "男"
    },
    {
        name: "王五",
        age: 20,
        sex: "男"
    },
])
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