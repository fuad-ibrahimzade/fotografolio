<template>
    <layout ref="parentComponent">
    </layout>
</template>

<script>
    import Layout from '@/components/layouts/Layout.vue';
    import AboutEdit from '@/components/dashboard/AboutEdit.vue';
    export default {
        name: "AboutEditUrl",
        components: {layout:Layout,AboutEdit:AboutEdit},
        mounted(){
            document.title = 'Photographer - AboutEdit'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(AboutEdit)
        }
    }
</script>

<style scoped>

</style>