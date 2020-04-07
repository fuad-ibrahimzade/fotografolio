<template>
    <layout ref="parentComponent">
    </layout>
</template>

<script>
    import Layout from '@/components/layouts/Layout.vue';
    import DashboardIndex from '@/components/dashboard/DashboardIndex.vue';
    export default {
        name: "DashboardIndexUrl",
        components: {layout:Layout,DashboardIndex:DashboardIndex},
        created(){
            // var isLoggedIn = $("meta[name=login-status]").attr('content');
            // if(!isLoggedIn){
            //     this.$router.push({path:'/login'})
            // }
            // this.$refs.parentComponent.isLoggedIn=isLoggedIn;
        },
        mounted(){
            document.title = 'Photographer - Dashboard'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(DashboardIndex)
        }
    }
</script>

<style scoped>

</style>