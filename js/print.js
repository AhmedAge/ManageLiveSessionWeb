function printDiv(divName) {
    //   $("#searchRow").hide();
    //   $("#printRow").hide();

     

                var printContents = document.getElementById("sutdentInfoDiv").innerHTML;
               var  NewprintContents =  " <div class='row'  style='padding-right: 0px!important;background-color:white!important;'>"+
   "             <img src='images/logo.png'  width='300px' style='float:right' /> "+
   "             <img src='images/logo-new.png' width='200px' style='float:left' /> "+
   "         </div>"+
" <script src='js/jquery-3.2.1.js'></script>   <script>$(function(){ $('#printRow').css('display','none');    })(); </script>";
                var styles="<style>th,td{ text-align: center;  vertical-align: middle!important;  }  </style>";
                var originalContents = document.body.innerHTML;
                document.body.innerHTML = styles+ NewprintContents+ printContents;
                window.print();
                document.body.innerHTML =  originalContents;
                window.close();
    //  $("#searchRow").show();
    //  $("#printRow").show();
}
/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


