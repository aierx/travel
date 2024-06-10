<script lang="ts" setup>
import HeaderView from "../components/view/HeaderView.vue";

</script>
<template>
    <div class="common-layout">
        <el-container>
            <el-header class="aaaheader">
                <HeaderView/>
            </el-header>
            <el-main style="width: 1300px; margin: 0 auto;">
                <router-view/>
            </el-main>
        </el-container>
    </div>
</template>

<style scoped>
body {
    margin: 0;
}

.aaaheader {
    border-bottom: 1px solid #dcdfe6;
    position: sticky;
    z-index: 100;
    background-color: #fff;
    background-size: 4px 4px;
    backdrop-filter: saturate(50%) blur(4px);
    display: flex;
    top: 0;
}
</style>
