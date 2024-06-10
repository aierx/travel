<script setup>
import {inject, ref} from "vue";
import {reactive} from "vue";
import router from "../router/router.js";
import {useUserInfoStore} from "../stores/user.js";
import {User} from "@element-plus/icons-vue";
import {ElMessage} from "element-plus";

const form = ref({
    phone: "",
    password: ""
});
const userInfo = useUserInfoStore()
const axios = inject("$axios")
const onSubmit = async () => {
    const data = await axios.post("/tour/user/login", form.value).then(e => e.data)
    if (data.code !== 200) {
        ElMessage({message: data.value, type: "error"})
    }
    let user = data.value
    user.isLogin = true
    userInfo.$patch(user)
    ElMessage({message: '登入成功', type: "success"})
    router.replace("/")
};

const dump = () => {
    router.replace("/register")
}

</script>

<template>
    <div style="height: calc(100vh);width: 100%;display: flex;position: relative;">
        <el-card style="width: 480px;position: absolute;top: 50%;left: 50%;transform: translate(-50%,-50%)">
            <el-form :model="form" label-width="auto" style="width: 440px;margin-top: 20px">
                <el-form-item
                    label="用户名"
                    :rules="[{ required: true, message: '用户名必填' }]"
                >
                    <el-input v-model="form.phone"/>
                </el-form-item>
                <el-form-item
                    label="密码"
                    :rules="[{ required: true, message: '密码必填' }]"
                >
                    <el-input v-model="form.password" type="password"/>
                </el-form-item>

                <el-form-item style="flex-direction: column;">
                    <el-button type="primary" @click="dump">注册</el-button>
                    <el-button type="success" @click="onSubmit">登入</el-button>
                </el-form-item>
            </el-form>
        </el-card>
    </div>


</template>

<style scoped></style>
