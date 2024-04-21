
import { defineStore } from 'pinia'

interface  tag{

    name:'',

    id:'',

}


export const tagsListName: Array<tag>=[

    {id:1, name:'Документы'},

    {id:2, name:'Учеба'},

    {id:3, name:'Реквизит'},

    {id:4, name:'Досуг'},

    {id:5, name:'Мероприятия'},

    {id:6, name:'Наука'},

    {id:7, name:'Развлечения'},

    {id:8, name:'Общежитие'},

    {id:9, name:'Разное'},

    {id:10, name:'Срочно'},

];



interface request{

    name:string,

    time:Date,

    tags:Array<tag>,

    comment:string,

    userId:number,

}

interface  myResponse{

    id:number,
    phone:string,
    comment:string,
    requestId:number,
    request:request,

}


export const useMainDataStore = defineStore('mainData',{

    state:()=>{

        return{

            dataHasBeenUploaded:false,

            userId:null as number|null,

            userName:'Даниэль',

            token:'',

            email:'',

            nameOfPage:'' as string,

            errorText:'',

            requestsList:[]  as Array<any>,


            //as Array<request>,

            myResponsesList:[] as Array<myResponse>,

            tagsList:[] as Array<tag>,

        }

    },

    actions:{

        setUserId(newUserId:number){

            this.userId=newUserId;

        },

        setToken(newToken:string){

            this.token=newToken;

        },

        setRequestsList(newRequestsList:Array<request>){

            this.requestsList=newRequestsList;

        },


        setMyResponsesList(newMyResponsesList:Array<myResponse>){

            this.myResponsesList=newMyResponsesList;


        },

        setNameOfPage(newNameOfPage:string){

            this.nameOfPage=newNameOfPage;

        },

        setErrorText(newTextError:string){

            this.errorText=newTextError;

            setTimeout(()=>{

                this.errorText='';

            }, 2000);

        },

        setVerificationText(newTextError:string) {

            this.errorText=newTextError;

        },

        removeVerificationText() {

            this.errorText='';

        },

    },

})