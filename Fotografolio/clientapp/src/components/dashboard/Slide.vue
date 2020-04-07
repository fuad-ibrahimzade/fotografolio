<template>
 <div>
  <div class="container">
   <div class="row justify-content-center">
    <div class="col-md-8">
     <div class="card">
      <div class="card-header">Slide Section</div>

      <div class="card-body">
       <div class="alert alert-success" role="alert" v-if="status">
        {{ status }}
       </div>

       <form method="post" action="/slide/new" enctype="multipart/form-data" @submit.prevent.stop="addPhotoMethod($event)">
        <!--<input type="hidden" name="_token" :value="csrf_token">-->
        <div class="form-row mb-2">
         <div class="col-12 col-md-9 mb-2">
          <input type="file" name="photos[]" accept="image/*" class="form-control" required multiple>
         </div>
         <div class="col">
          <input type="submit" value="Add Photos" class="form-control btn btn-primary">
         </div>
        </div>
       </form>


       <div class="mb-2 text-center" v-if="slide">
        <span style="position: relative" v-for="photo in slide.slice().reverse()" :key="photo.id">
         <img :src="photo.name" width="150" height="150" class="mb-3 mr-2">
         <div style="position: absolute; top: -70px; right:13px;">
         <form method="post" :action="'/slide/'+photo.id+'/delete'" @submit.prevent.stop="destroyPhotoMethod($event)">
         <!--<input type="hidden" name="_token" :value="csrf_token">-->
          <input type="button" class="ajax-submit" style="display: none" @click="destroyPhotoMethod($event)">
         <input type="hidden" name="_method" value="DELETE">
         <button class="btn btn-danger" name="remove_levels" type="submit"><i class="fa fa-trash"></i></button>
         </form>
         </div>
        </span>
       </div>


      </div>
     </div>
    </div>
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

  <!--<script type="application/javascript">
      $(document).ready(function(){

          $('button[name="remove_levels"]').on('click', function(e) {
              var $form = $(this).closest('form');
              e.preventDefault();
              $('#confirm').modal({
                  backdrop: 'static',
                  keyboard: false
              })
                  .one('click', '#delete', function(e) {
                      // $form.trigger('submit');
                      $($form).find('.ajax-submit').trigger('click');
                  });
          });

          setTimeout(function() {
              $('#successMessage').fadeOut('fast');
          }, 3000);



      });
  </script>-->
  <script type="application/javascript" v-if="fileError">
      $('#fileError').modal('show');
  </script>
 </div>
</template>

<script>
    export default {
        name: "Slide",
        data() {
            return {
                csrf_token:this.$root.csrf_token,
                api_token:this.$root.api_token,
                loading: false,
                error: null,
                fileError:null,
                addPhoto:null,
                destroyPhoto:null,
                status:null,
                slide:null,
            };
        },
        mounted(){
            this.startGettingData()
        },
        methods:{
            addPhotoMethod(event){
                this.$parent.loading=true;
                var form=$(event.target)
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
            startGettingData(){
                this.loading=true;
                axios
                    .get('api/slide',{
                        headers: {
                            'X-CSRF-TOKEN': this.csrf_token,
                            'X-Requested-With': 'XMLHttpRequest',
                            'Authorization': 'Bearer ' + this.api_token,
                        }
                    })
                    .then(response => {
                        this.loading=false;
                        this.slide = response.data.slide;
                    }).catch(error => {
                    this.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
        }
    }
</script>

<style scoped>

</style>