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
    selected_aggreg : ko.observable({}),
    currentAxes : ko.observableArray([]),
    loading : ko.observable(false),
    select_aggreg_axes : ko.observableArray([]),
    select_aggreg : function (data, event){
        viewModel.selected_aggreg(data);
        viewModel.select_aggreg_axes(data["Axes"]);
    }

};

viewModel.has_selected_aggreg = new ko.computed(function(){
    return viewModel.selected_aggreg()!= {};
}, viewModel);

ko.applyBindings(viewModel);

})();