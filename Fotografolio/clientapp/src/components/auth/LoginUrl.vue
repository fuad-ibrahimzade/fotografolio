<template>
    <layout ref="parentComponent">
    </layout>
</template>

<script>
    import Layout from '@/components/layouts/Layout.vue';
    import Login from '@/components/auth/Login.vue';
    export default {
        name: "LoginUrl",
        components: {layout:Layout,login:Login},
        mounted(){
            document.title = 'Photographer - Login'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(Login)
        }
    }
</script>

<style scoped>

</style>