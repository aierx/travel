<template>
  <div style="height: calc(100vh); width: 100%; position: relative">
    <el-card
      style="
        width: 480px;
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
      "
    >
      <el-form
        :model="userInfo"
        label-width="auto"
        style="max-width: 440px; margin-top: 20px"
      >
        <el-form-item label="姓名">
          <el-input v-model="userInfo.name" />
        </el-form-item>
        <el-form-item label="用户名">
          <el-input v-model="userInfo.aliasName" />
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
          :rules="[{ required: true, message: '出生日期必填' }]"
        >
          <el-date-picker
            v-model="userInfo.birthDate"
            type="date"
            placeholder="Pick a day"
            size="default"
          />
        </el-form-item>
        <el-form-item label="手机号">
          <el-input v-model="userInfo.phone" />
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="userInfo.email" />
        </el-form-item>
        <el-form-item label="地址">
          <el-input v-model="userInfo.address" />
        </el-form-item>
        <el-form-item label="密码">
          <el-input type="password" v-model="userInfo.passwd" />
        </el-form-item>
        <el-form-item label="重复密码">
          <el-input type="password" v-model="userInfo.passwd2" />
        </el-form-item>
        <el-form-item style="flex-direction: column">
          <el-button type="success" @click="register">注册</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup>
import { inject, ref } from "vue";
import router from "../router/router.js";
import { ElMessage } from "element-plus";
import { formatDate } from "../utils.js";

// do not use same name with ref
const userInfo = ref({});
const axios = inject("$axios");
const register = async () => {
  if (userInfo.value.passwd !== userInfo.value.passwd2) {
    ElMessage({ message: "密码不一致", type: "error" });
  }
  userInfo.value.birthDate = formatDate(userInfo.value.birthDate);
  let res = await axios
    .post("tour/user/save", userInfo.value)
    .then((e) => e.data);
  if (res.code !== 200) {
    ElMessage({ message: res.value, type: "error" });
  } else {
    ElMessage({ message: "注册用户成功", type: "success" });
    router.replace("/login");
  }
};
</script>
