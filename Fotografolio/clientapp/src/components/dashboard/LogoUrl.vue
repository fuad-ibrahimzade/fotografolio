<template>
    <layout ref="parentComponent">
    </layout>
</template>

<script>
    import Layout from '@/components/layouts/Layout.vue';
    import Logo from '@/components/dashboard/Logo.vue';
    export default {
        name: "LogoUrl",
        components: {layout:Layout,Logo:Logo},
        mounted(){
            document.title = 'Photographer - Logo'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(Logo)
        }
    }
</script>

<style scoped>

</style>