<template>
    <homelayout ref="parentComponent">
    </homelayout>
</template>

<script>
    import HomeLayout from '@/components/home/HomeLayout.vue';
    import Gallery from '@/components/home/Gallery.vue';
    export default {
        name: "GalleryUrl",
        components: {homelayout:HomeLayout,gallery:Gallery},
        mounted(){
            document.title = 'Photographer - Gallery'
            // var currentUrl = window.location.pathname;
            // currentUrl=this.$route.path;
            // console.log(currentUrl);
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(Gallery)
            // this.$refs.parentComponent.$refs.childComponent.startGettingData($route.params.gallery)
        }
    }
</script>

<style scoped>

</style>