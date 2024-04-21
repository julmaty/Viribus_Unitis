<script lang="ts">

import {computed, defineComponent} from "vue";
import {useMainDataStore} from "@/store/mainData";
import axios from "axios";

export default defineComponent({

  setup(){

    const mainDataStore=useMainDataStore();

    const myResponsesList= computed(()=>mainDataStore.myResponsesList)

    return{

      mainDataStore,

      myResponsesList,

    }

  },


  data(){

    return{

      responseId:0,

      request:null,

      response:null,


    }
  },

  created(){

    this.responseId=Number(this.$route.params.id);

   //this.request=this.mainDataStore.myResponsesList.filter((el)=>el.id==this.responseId).request;

    this.response=this.myResponsesList.filter((el)=>el.id==this.responseId)[0];

    this.request=this.response.request;

    console.log(this.request);

  },


  methods:{

    deleteResponse(){

      axios.delete(`https://viribusunitis.ru/api/responses/${this.responseId}`);

      this.mainDataStore.myResponsesList=this.mainDataStore.myResponsesList.filter((el)=>el.id!=this.responseId);

      this.$router.push({replace:true, path:'/myRequestsResponses'});

    },

  },

});

</script>

<template>

  <div class="response" v-if="request!=null">

    <router-link class="backButton" to="/myRequestsResponses"><img src="@/assets/leftBottom.svg"></router-link>

    <div class="userFullName" v-if="$route.name!='myRequest'">Заказчик: {{ request.userName }}</div>

    <div class="name">Название запроса: {{ request.name }}</div>

    <!--<div class="time">Размещено {{ requestDate(request.createdDate) }}</div>-->

    <div class="tags-list">

      <div class="tag" v-for="tagId in request.tags">{{ searchTagNameById(tagId) }}</div>

    </div>

    <div class="comment">Описание запроса: {{request.description}}</div>

    <div

        class="respondButton"

        v-on:click="deleteResponse"

    >Удалить</div>

  </div>


</template>

<style scoped>

.response{

  .backButton{

    font-size:20px;
    cursor:pointer;
    display:block;

  }

  .userFullName{

    font-size:20px;
    margin-top:20px;
    margin-bottom:20px;

  }

  .name{

    font-size:20px;
    margin-top:20px;
    margin-bottom:20px;

  }

  .time{

    font-size:12px;
    color:gray;
    margin-top:10px;

  }


  .comment{

    font-size:18px;

  }

  .tags-list{

    display:flex;
    margin-top:20px;
    margin-bottom:20px;
    flex-wrap: wrap;

    .tag{

      margin-left:10px;
      margin-right:10px;
      color:red;
      border-width: 1px;
      border-color:red;
      border-style:solid;
      border-radius:10px;
      padding:5px;
      font-size:18px;
      margin-bottom:10px;

    }

  }

  .respondButton{

    margin-left:auto;
    margin-right:auto;
    margin-top:30px;
    widht:130px;
    background-color: red;
    font-size:20px;
    border-radius:20px;
    height:50px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    margin-bottom:60px;
    color:white;
    background-color:#3643B3;

  }

}


</style>