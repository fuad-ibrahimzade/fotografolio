export default function auth({ next, router }) {
    // if (!localStorage.getItem('jwt')) {
    //     return router.push({ name: 'login' });
    // }
    var isLoggedIn = $("meta[name=login-status]").attr('content');
    if(!isLoggedIn){
        this.$router.push({path:'/login'})
    }

    return next();
}