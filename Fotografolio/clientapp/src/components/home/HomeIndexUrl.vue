<template>
    <homelayout ref="parentComponent">
    </homelayout>
</template>

<script>
    import HomeLayout from '@/components/home/HomeLayout.vue';
    import HomeIndex from '@/components/home/HomeIndex.vue';
    export default {
        name: "HomeIndexUrl",
        components: {homelayout:HomeLayout,homeindex:HomeIndex},
        mounted(){
            document.title = 'Photographer - Index'
            this.$refs.parentComponent.startGettingData()
            this.$refs.parentComponent.swapComponent(HomeIndex)
            // let script1 = document.createElement('script')
            // script1.setAttribute('src', '/common-js/jquery-3.2.1.min.js')
            // script1.type='text/javascript'
            // document.head.appendChild(script1)
            // let script2 = document.createElement('script')
            // script2.setAttribute('src', '/common-js/scripts.js')
            // document.head.appendChild(script2)
            //

            // let script1 = document.createElement('script')
            // script1.setAttribute('src', '/common-js/tether.min.js')
            // document.head.appendChild(script1)
            // let script2 = document.createElement('script')
            // script2.setAttribute('src', '/common-js/bootstrap.js')
            // document.head.appendChild(script2)
            // let script3 = document.createElement('script')
            // script3.type='text/javascript'
            // script3.setAttribute('src', '/revolution/js/jquery.themepunch.tools.min.js')
            // document.head.appendChild(script3)
            // let script4 = document.createElement('script')
            // script4.type='text/javascript'
            // script4.setAttribute('src', '/revolution/js/jquery.themepunch.revolution.min.js')
            // document.head.appendChild(script4)
        }
    }
</script>

<style scoped>

</style>