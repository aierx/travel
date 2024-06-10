<script setup>
import {onMounted, ref} from "vue";

import {UserFilled,Guide,OfficeBuilding,Document} from "@element-plus/icons-vue";
import router from "../../router/router.js";
import {useUserInfoStore} from "../../stores/user.js";

function select(e) {
    if (e==="user"){
        router.replace("/admin/user")
    }
    if (e==="scenery"){
        router.replace("/admin/scenery")
    }
    if (e==="hotel"){
        router.replace("/admin/hotel")
    }
    if (e==="order"){
        router.replace("/admin/order")
    }
}
const user = useUserInfoStore()

onMounted(()=>{
    user.page = "admin"
})
</script>

<template>
    <div>
        <el-row :gutter="20">
            <el-col :span="3">
                <el-menu
                    :default-active="user.secondPage"
                    class="el-menu-vertical-demo"
                    @select="select"
                    style="height: calc(100vh - 100px)"
                >
                    <el-menu-item index="user" >
                        <el-icon>
                            <UserFilled/>
                        </el-icon>
                        <span> 用户管理</span>
                    </el-menu-item>
                    <el-menu-item index="scenery">
                        <el-icon><Guide /></el-icon>
                        <span>景点管理</span>
                    </el-menu-item>
                    <el-menu-item index="hotel">
                        <el-icon><OfficeBuilding /></el-icon>
                        <span>酒店管理</span>
                    </el-menu-item>
                    <el-menu-item index="order">
                        <el-icon>
                            <document/>
                        </el-icon>
                        <span>订单管理</span>
                    </el-menu-item>
                </el-menu>

            </el-col>
            <el-col :span="21">
                <router-view/>
            </el-col>
        </el-row>
    </div>
</template>

<style scoped>

</style>