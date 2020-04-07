<template>
 <div>
   <div class="container">
    <div class="row justify-content-center">
     <div class="col-md-8">
      <div class="card">
       <div class="card-header">Gallery Section</div>

       <div class="card-body">
        <div class="alert alert-success" role="alert" v-if="status">
         {{ status }}
        </div>

        <form method="post" action="/gallery/new" @submit.prevent.stop="newGalleryMethod($event)">
         <!--<input type="hidden" name="_token" :value="csrf_token">-->
         <div class="form-row mb-2">
          <div class="col-12 col-md-9 mb-2">
           <input type="text" name="name" maxlength="100" class="form-control" placeholder="Create New Gallery ..." required>
          </div>
          <div class="col">
           <button type="submit" class="form-control btn btn-primary">Confirm</button>
          </div>
         </div>
        </form>

        <table class="table table-bordered" v-if="galleries">
         <thead>
         <tr>
          <th scope="col">Gallery</th>
          <th scope="col">Delete</th>
         </tr>
         </thead>
         <tbody v-for="gallery in galleries.slice().reverse()" :key="gallery.id" v-if="galleries">
         <!--<tr>-->
             <tr scope="row">
              <td :id="gallery.id" class="col">{{ gallery.name }}</td>
              <td class="col">
               <form class="float-right" method="post" :action="'/gallery/'+gallery.id+'/delete'" @submit.stop.prevent="destroyGalleryMethod($event)">
                <!--<input type="hidden" name="_token" :value="csrf_token">-->
                <input type="button" class="ajax-submit" style="display: none" @click="destroyGalleryMethod($event)">
                <input type="hidden" name="_method" value="DELETE">

                <button class="btn btn-danger" name="remove_levels" type="submit" ><i class="fa fa-trash"></i></button>
               </form>
              </td>
             </tr>
             <tr>
              <td colspan="4" :class="'subContent '+gallery.id">

               <form action="/photo/new" method="post" enctype="multipart/form-data" @submit.prevent.stop="addPhotoMethod($event)">
                <!--<input type="hidden" name="_token" :value="csrf_token">-->
                <input type="hidden" name="gallery_id" :value="gallery.id">
                <div class="form-row mb-2">
                 <div class="col-12 col-md-9 mb-2">
                  <input type="file" name="photos[]" accept="image/*" class="form-control" required multiple>
                 </div>
                 <div class="col">
                  <input type="submit" value="Add Photos" class="form-control btn btn-success" >
                 </div>
                </div>
               </form>
               <div class="mb-2 text-center" v-for="photo in gallery.photos.slice().reverse()" :key="photo.id" v-if="gallery && gallery.photos">
                <span style="position: relative">
                     <img :src="photo.name" width="150" height="150" class="mb-3 mr-2">
                     <div style="position: absolute; top: -70px; right:13px;">
                     <form method="post" :action="'/photo/'+photo.id+'/delete'" @submit.stop.prevent="destroyPhotoMethod($event)">
                     <!--<input type="hidden" name="_token" :value="csrf_token">-->
                     <input type="button" class="ajax-submit" style="display: none" @click="destroyPhotoMethod($event)">
                     <input type="hidden" name="_method" value="DELETE">
                     <button class="btn btn-danger" name="remove_levels" type="submit" ><i class="fa fa-trash"></i></button>
                     </form>
                     </div>
                 </span>
               </div>


              </td>
             </tr>
             <input type="hidden" id="photos" :value="gallery.photos">
         </tbody>
        </table>
       </div>
      </div>
     </div>
    </div>
   </div>

   <div v-if="createGallery">
       <div class="alert alert-success w-alert fixed-bottom ml-auto mr-2" id="successMessage" v-model="createGallery">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> {{ createGallery }}
       </div>
   </div>
   <div v-if="destroyGallery">
       <div class="alert alert-success w-alert fixed-bottom ml-auto mr-2" id="successMessage" v-model="destroyGallery">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> {{ destroyGallery }}
       </div>
   </div>
   <div v-if="addPhoto">
       <div class="alert alert-success w-alert fixed-bottom ml-auto mr-2" id="successMessage">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> {{ addPhoto }}
       </div>
   </div>
   <div v-if="destroyPhoto">
       <div class="alert alert-success w-alert fixed-bottom ml-auto mr-2" id="successMessage">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> {{ destroyPhoto }}
       </div>
   </div>




   <!-- Modal -->
   <div class="modal fade" id="confirm" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
     <div class="modal-content">

      <div class="modal-body">
       Are you sure?
      </div>
      <div class="modal-footer">
       <button type="button" class="btn btn-danger" data-dismiss="modal" id="delete">Delete</button>
       <button type="button" class="btn" data-dismiss="modal">Cancel</button>
      </div>
     </div>
    </div>
   </div>

   <div class="modal fade" id="fileError" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
     <div class="modal-content">
      <div class="modal-header">
       <h5 class="modal-title">Yükləməniz uğursuz oldu!</h5>
       <button type="button" class="close" data-dismiss="modal" aria-label="Close">
        <span aria-hidden="true">&times;</span>
       </button>
      </div>
      <div class="modal-body">
       {{ fileError }}
      </div>
     </div>
    </div>
   </div>

 </div>
</template>

<script>
    import axios from 'axios';
    //axios.withCredentials = true;
    //axios.defaults.withCredentials = true;

    export default {
        name: "DashboardIndex",
        data() {
            return {
                csrf_token:this.$root.csrf_token,
                api_token:this.$root.api_token,
                loading: false,
                error: null,
                fileError:null,
                createGallery:null,
                destroyGallery:null,
                addPhoto:null,
                destroyPhoto:null,
                status:null,
                galleries:null,
            };
        },
        mounted(){
            // let script1 = document.createElement('script')
            // script1.setAttribute('src', 'https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js')
            // document.getElementById('scripts').appendChild(script1)

            // let script2 = document.createElement('script')
            // script2.type='text/javascript'
            // script2.text=`
            //     $(document).ready(function(){
            //         $(".subContent").hide();
            //
            //         for(let gallery in this.galleries) {
            //             $("#"+gallery.id).click(function () {
            //                 $("."+gallery.id).toggle();
            //             });
            //         }
            //
            //         $('button[name="remove_levels"]').on('click', function(e) {
            //             var $form = $(this).closest('form');
            //             e.preventDefault();
            //             $('#confirm').modal({
            //                 backdrop: 'static',
            //                 keyboard: false
            //             })
            //                 .one('click', '#delete', function(e) {
            //                     $form.trigger('submit');
            //                 });
            //         });
            //
            //         setTimeout(function() {
            //             $('#successMessage').fadeOut('fast');
            //         }, 3000);
            //
            //         if(this.fileError) {
            //             $('#fileError').modal('show');
            //         }
            //     });
            // `
            // document.getElementById('scripts').appendChild(script2)

            this.startGettingData()
        },
        methods:{
            addPhotoMethod(event){
                this.$parent.loading=true;
                var form=$(event.target)
                // form.submit();
                axios({
                    url: '/api' + $(form).attr('action'),
                    method: 'post',
                    headers: {
                        'X-CSRF-TOKEN':this.csrf_token,
                        'X-Requested-With':'XMLHttpRequest',
                        'Authorization':'Bearer ' + this.api_token,
                    },
                    data:new FormData(event.target)
                })
                    .then(response => {
                        this.$parent.loading=false;
                        if(response.data.hasOwnProperty('addPhoto')){
                            this.addPhoto=response.data.addPhoto
                        }
                        if(response.data.hasOwnProperty('fileError')){
                            this.fileError=response.data.fileError
                        }
                        this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            destroyPhotoMethod(event){
                this.$parent.loading=true;
                var form = $(event.target)
                    //.parent()
                // form.submit();
                console.log( $(form).attr('action'))
                axios({
                    url: '/api' + $(form).attr('action'),
                    method: 'delete',
                    headers: {
                        'X-CSRF-TOKEN':this.csrf_token,
                        'X-Requested-With':'XMLHttpRequest',
                        'Authorization':'Bearer ' + this.api_token,
                    },
                })
                    .then(response => {
                        this.$parent.loading=false;
                        this.destroyPhoto= response.data.destroyPhoto;
                        this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            destroyGalleryMethod(event){
                this.$parent.loading=true;
                var form = $(event.target)
                    //.parent()
                // form.submit();
                console.log( $(form).attr('action'))
                axios({
                    url: '/api' + $(form).attr('action'),
                    method: 'delete',
                    headers: {
                        'X-CSRF-TOKEN':this.csrf_token,
                        'X-Requested-With':'XMLHttpRequest',
                        'Authorization':'Bearer ' + this.api_token,
                    },
                })
                    .then(response => {
                        this.$parent.loading=false;
                        this.destroyGallery= response.data.destroyGallery;
                        this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            newGalleryMethod(event){
                this.$parent.loading=true;
                var form=$(event.target)
                // $(form).serialize()
                // form.submit();
                axios({
                        url: '/api' + $(form).attr('action'),
                        method: 'post',
                        headers: {
                        'X-CSRF-TOKEN':this.csrf_token,
                        'X-Requested-With':'XMLHttpRequest',
                        'Authorization':'Bearer ' + this.api_token,
                        },
                        data:new FormData(event.target)
                    })
                    .then(response => {
                        this.$parent.loading=false;
                        this.createGallery= response.data.createGallery;
                        this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            startGettingData(){
                this.$parent.loading=true;
                axios
                    .get('/api/dashboard',{
                        headers: {
                            'X-CSRF-TOKEN': this.csrf_token,
                            'X-Requested-With': 'XMLHttpRequest',
                            'Authorization': 'Bearer ' + this.api_token,
                        }
                    })
                    .then(response => {
                        this.$parent.loading=false;
                        const oldGalleryNotNULL=this.galleries!==null?true:false;
                        // var OLDgalleries=this.galleries;
                        this.galleries = response.data.galleries;
                        $(document).ready(function(){
                            $(".subContent").hide();
                            if(oldGalleryNotNULL){
                                for(var i3=0, len=response.data.galleries.length; i3 < len; i3++){
                                    let gallery=response.data.galleries[i3];
                                    $("#"+gallery.id).off('click')
                                }
                            }

                            for(var i=0, len=response.data.galleries.length; i < len; i++){
                                let gallery=response.data.galleries[i];
                                // let OLDgalleryEXIST=false
                                // for(var i2=0, len=OLDgalleries.length; i2 < len; i2++){
                                //     if(gallery.id==OLDgalleries[i2].id){
                                //         OLDgalleryEXIST=true
                                //         break;
                                //     }
                                // }
                                // if(OLDgalleryEXIST)continue;
                                $("#"+gallery.id).click(function () {
                                    $("."+gallery.id).toggle();
                                });
                            }

                            //if(oldGalleryNotNULL){
                            //    $('button[name="remove_levels"]').off('click')
                            //}
                            //$('button[name="remove_levels"]').on('click', function(e) {
                            //    var $form = $(this).closest('form');
                            //    e.preventDefault();
                            //    $('#confirm').modal({
                            //        backdrop: 'static',
                            //        keyboard: false
                            //    })
                            //        .one('click', '#delete', function(e) {
                            //            // $form.trigger('submit');
                            //            $($form).find('.ajax-submit').trigger('click');
                            //        });
                            //});

                            //setTimeout(function() {
                            //    $('#successMessage').fadeOut('fast');
                            //}, 3000);

                            if(this.fileError) {
                                $('#fileError').modal('show');
                            }


                        });
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
        }
    }
</script>

<style scoped>

</style>