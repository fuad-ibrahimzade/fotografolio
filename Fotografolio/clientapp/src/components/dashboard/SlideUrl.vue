<template>
    <layout ref="parentComponent">
    </layout>
</template>

<script>
    import Layout from '@/components/layouts/Layout.vue';
    import Slide from '@/components/dashboard/Slide.vue';
    export default {
        name: "SlideUrl",
        components: {layout:Layout,Slide:Slide},
        mounted(){
            document.title = 'Photographer - Slide'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(Slide)
        }
    }
</script>

<style scoped>

</style>