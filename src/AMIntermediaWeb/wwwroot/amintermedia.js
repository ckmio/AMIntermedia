(function (){

function handleMessage(strMessage){
    if(strMessage.startsWith("order-update:")){
        viewModel.aggregations.push(JSON.parse(strMessage.substring("order-update:".length)));
        return;
    }
}


var s = new WebSocket("ws://" + window.location.host + "/ws");
s.onmessage = (message)=> {
    console.log(message.data);
    handleMessage(message.data);
}
s.onopen = ()=>{
    s.send("hello");
}


var viewModel = {
    aggregations : ko.observableArray([]),
    currentAxes : ko.observableArray([]),
    loading : ko.observable(false),
};

ko.applyBindings(viewModel);

})();