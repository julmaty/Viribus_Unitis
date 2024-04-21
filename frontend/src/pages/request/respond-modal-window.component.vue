<script lang="ts">

import {computed, defineComponent} from "vue";
import axios from "axios";
import {useMainDataStore} from "@/store/mainData";

export default defineComponent({

  setup(){

    const mainDataStore=useMainDataStore();

    const setErrorText=mainDataStore.setErrorText;

    const userId=computed(()=>mainDataStore.userId);

    return{

      mainDataStore,

      setErrorText,

      userId,



    }

  },

  props:{

    requestId:Number,

  },

  data(){
    return{

      comment:'',

      phone:'',

      name:'',

    }
  },

  methods:{

    async sendRespond(){

      //this.setErrorText();

      if(this.comment.length==0){

        this.setErrorText('Введите комментарий');

      }else if( this.phone.length==0){

        this.setErrorText('Введите свой телефон');

      }else if(this.name.length==0){

        this.setErrorText('Введите свое имя');

      }else{

        let response=await  axios.post('https://viribusunitis.ru/api/responses',{

          RequestId:this.requestId,

          UserId:this.userId,

          Comment:this.comment,

          Phone:this.phone,

          UserName:this.name,

        });

        console.log(response);

        this.$router.push({replace:true, path:'/'});


      /*

      this.$emit('sendedRespond');
      setTimeout(()=>{

        this.$router.push({path:'/'});


      }, 300);

      */



      }








    },

    closeModalWindow(){

      this.$emit('closeModalWindow');

    },

  }


});

</script>

<template>

  <div class="respondModalWindow">

    <div class="close" v-on:click="closeModalWindow">Закрыть</div>

    <div class="fieldName">Комментарий:</div>

    <textarea class="fieldInput" v-model="comment" placeholder="Напишите тут свой комментарий к ответу на запрос"></textarea>

    <div class="fieldName">Телефон для связи:</div>

    <input v-model="phone" placeholder="Введите свой телефон для связи" class="fieldInput">

    <div class="fieldName">Ваше имя:</div>

    <input v-model="name" placeholder="Ввведите ваше имя" class="fieldInput">

    <div class="sendRespond" v-on:click="sendRespond">Отправить отклик</div>

  </div>

</template>

<style scoped>

.respondModalWindow{

  background-color:#EDEDED;

  textarea{

    resize:none;
    background-color:#EDEDED;
  }

  padding-top:20px;
  padding-left:15px;
  padding-right:15px;

  .close{

    font-size:20px;
    cursor:pointer;
    display:block;
    margin-bottom:20px;

  }

  position:absolute;
  bottom:0px;
  left:0px;
  width:100%;
  height:380px;

  display:block;

  border-color:black;
  //border-top-width:2px;
  //border-left-width:2px;
  //border-right-width:2px;
  border-style:solid;
  border-top-left-radius:20px;
  border-top-right-radius:20px;

  .fieldName{

    font-size:20px;
    margin-bottom:10px;
    width:100%;

  }

  .fieldInput{

    font-size:20px;
    margin-bottom:10px;
    width:100%;

    border-width:0px 0px 1px 0px;
    border-color:black;
    border-style:solid;
    background-color:#EDEDED;

  }

  .sendRespond{

    margin-left:auto;
    margin-right:auto;
    margin-top:30px;
    widht:130px;
    background-color:#3643B3;
    font-size:20px;
    border-radius:20px;
    height:50px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    color:white;

  }

}


</style>