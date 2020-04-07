<template>
    
</template>

<script>
    export default {
        name: "auth.vue",
        beforeRouteEnter(to, from, next){
            var isLoggedIn = $("meta[name=login-status]").attr('content');
            if(!isLoggedIn){
                this.$router.push({path:'/login'})
            }
            return next();
        }
    }
</script>

<style scoped>

</style>