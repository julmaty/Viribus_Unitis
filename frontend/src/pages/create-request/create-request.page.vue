<script lang="ts">

import {computed, defineComponent} from "vue";

import {tagsListName, useMainDataStore} from "@/store/mainData.js";
import axios from "axios";
//import { TogetherAI } from "@langchain/community/llms/togetherai";

import { HumanMessage, AIMessage, SystemMessage } from "@langchain/core/messages";
import { ChatOpenAI } from "@langchain/openai";

import { ChatTogetherAI } from "@langchain/community/chat_models/togetherai";

export default defineComponent({

  setup(){

    const mainDataStore=useMainDataStore();

    const setErrorText=mainDataStore.setErrorText;

    const userId=computed(()=>mainDataStore.userId);

    const token=computed(()=>mainDataStore.token);

    const email=computed(()=>mainDataStore.email);

    const setVerificationText=mainDataStore.setVerificationText;

    const removeVerificationText=mainDataStore.removeVerificationText;


    return{

      mainDataStore,

      setErrorText,

      userId,

      token,

      email,

      setVerificationText,

      removeVerificationText,

    }

  },

  data(){

    return{

      name:'',

      userName:'',

      comment:'',

      tagsList:tagsListName,

      LLM:new ChatTogetherAI({

        //streaming: false,

        //togetherAIApiKey: 'a6300585f5f374fd31ee27db2d01ca30feb7ffa324f5950633bdce4e805ac434',

        //modelName:"openchat/openchat-3.5-1210",

        modelName:"meta-llama/Llama-3-8b-chat-hf",

        apiKey: 'a6300585f5f374fd31ee27db2d01ca30feb7ffa324f5950633bdce4e805ac434',

        maxTokens: 200,

        temperature: 0,

      }),

    }
  },

  created(){

    //this.userName=this.mainDataStore.userName;

  },

  methods:{

    async checkString(checkedString:string, templateString:string){

      //Получаем ответ от робота:
      let response=await this.LLM.call([new HumanMessage(checkedString), new HumanMessage(`Напиши есть ли в сообщении выше смысл на тему

     ${templateString}.Ответь просто "да" или "нет"

    `)]);

      console.log(response.content);

      if(response.content.includes('Да')){

        return false

      }
      else{

        return true
      }

    },


    async sendRequest(){

      if(this.name.length==0){

        this.setErrorText('Введите название задания');

      }else if( this.userName.length==0){

        this.setErrorText('Введите свое имя');

      }else if(this.comment.length==0){

        this.setErrorText('Введите описание задания');

      }else{

        this.setVerificationText('Идет модерация запроса');

        //console.log(await this.checkString('Жопа','ругательства или нецензурная лексика'));

        //let problem1:boolean=await this.checkString('Война, убийства','жестокость');

        //console.log(problem1);


       //let problem2:boolean=await this.checkString('тупой идиот ты','ругательства или нецензурная лексика');

        //console.log(problem2);


       let problem1:boolean=await this.checkString(this.comment,'ругательства или нецензурная лексика');

       let problem2:boolean=await this.checkString(this.comment,'жестокость');

       if(!problem1 || !problem2){

       this.setErrorText('Ваш запрос НЕ прошел модерацию');

      }else{

       this.setErrorText('Ваш запрос прошел модерацию');

        //Если с модерацией запроса все ок, то разрешаем его закидывать в базу данных
        let selectedTags=this.tagsList.filter((tag)=>tag.highlighted==true).map(tag=>{

          return tag.id;

        });

          let response=await  axios.post('https://viribusunitis.ru/api/requests',{

            Name:this.name,

            Description:this.comment,

            UserId:this.userId,

            Tags:selectedTags,

            Email:this.email,

            UserName:this.userName,

          });

          //this.mainDataStore.requestsList.push();

          console.log(response);
          this.$router.push({replace:true, path:'/'});

        }

      }

    },

    selectTag(id:number){

      let highlightedIndex=this.tagsList.findIndex(tag => tag.id === id);

      if(this.tagsList[highlightedIndex].highlighted!=true){

        this.tagsList.splice(highlightedIndex, 1,{id:id, name:this.tagsList[highlightedIndex].name, highlighted:true});


      }else{

        this.tagsList.splice(highlightedIndex, 1,{id:id, name:this.tagsList[highlightedIndex].name, highlighted:false});


      }


    },

  },

});

</script>

<template>

  <div class="create-response">

    <router-link class="backButton" to="/"><img src="@/assets/leftBottom.svg"></router-link>

    <textarea class="name" placeholder="Название" v-model="name"></textarea>

    <textarea placeholder="Имя пользователя" v-model="userName"></textarea>

    <textarea class="comment" placeholder="Подробное описание и инструкции для исполнителей" v-model="comment"></textarea>

    <div class="tags-list">

      <div

          class="tag"

          v-for="tag in tagsList"

          v-on:click="selectTag(tag.id)"

          :class="{selectedTag:tag.highlighted==true}"

      >{{tag.name}}</div>

    </div>

    <div class="requestButton" v-on:click="sendRequest">Отправить просьбу</div>

  </div>

</template>

<style scoped>

.create-response{

  .backButton{

    font-size:20px;
    cursor:pointer;
    display:block;

  }

  textarea{

    resize:none;
    font-size:20px;
    margin-bottom:10px;
    width:100%;

    border-color:black;
    border-width:0px;
    border-style:solid;
    padding:10px;

    border-radius:8px;
    background-color:#EDEDED;

  }

  .name{

    margin-top:20px;

  }

  .comment{

    height:200px;

  }

  .tags-list{

    display:flex;
    margin-top:20px;
    margin-bottom:10px;
    flex-wrap: wrap;
    cursor:pointer;

    .tag{

      margin-left:10px;
      margin-right:10px;
      border-width: 1px;
      border-style:solid;
      border-radius:10px;
      padding:5px;
      font-size:18px;
      margin-bottom:10px;

    }

    .selectedTag{

      color:#3643B3;
      border-color:#3643B3;

    }

  }

  .requestButton{

    margin-left:auto;
    margin-right:auto;
    margin-top:10px;
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