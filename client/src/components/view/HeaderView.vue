<script setup>
import {inject} from "vue";
import {useUserInfoStore} from '../../stores/user.js';
import router from "../../router/router.js";
import {ElMessage} from "element-plus";

const user = useUserInfoStore();
const axios = inject('$axios')
const transValue = async (e) => {

    if (e === "admin") {
        await router.replace("/admin")
        user.page = e

    }
    if (e === "product") {
        await router.replace("/product")
        user.page = e
    }
    if (e === "user-center") {
        if (user.isLogin) {
            await router.replace("/user")
            user.page = e
        } else {
            await router.replace("/login")
        }
    }
    // 退出
    if (e === "user-logout") {
        user.$reset()
        await axios.post("tour/user/logout")
        await router.push("/login")
        ElMessage({message: "退出成功", type: "success"})
    }
}

</script>
<template>
    <el-row style="
          width: 1300px;
          height: 55px;
          margin: auto;
          align-content: center;
          justify-content: space-between;
        ">
        <div class="box" style=" display: flex;align-items: center;justify-content: center">
            旅游 酒店
        </div>
        <el-menu
            :default-active="user.page"
            class="el-menu-demo"
            mode="horizontal"
            @select="transValue"
            :ellipsis="false"
        >
            <el-menu-item index="admin" v-show="user.isLogin && user.isAdmin">管理</el-menu-item>
            <el-menu-item index="product">首页</el-menu-item>
            <el-sub-menu index="user">
                <template #title>{{ user.aliasName === '' ? "请登入" : user.aliasName }}</template>
                <el-menu-item index="user-center">{{ user.isLogin ? '个人中心' : '登入' }}</el-menu-item>
                <el-menu-item index="user-logout" v-show="user.isLogin">退出</el-menu-item>
            </el-sub-menu>
        </el-menu>
    </el-row>
</template>