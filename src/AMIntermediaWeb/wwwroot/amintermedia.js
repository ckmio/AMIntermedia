(function (){

function handleMessage(strMessage){
    if(strMessage.startsWith("order-update:")){
        var newAggreg = JSON.parse(strMessage.substring("order-update:".length));
        newAggreg.selected = ko.observable(false);
        viewModel.aggregations.push(newAggreg);
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
    selected_aggreg : ko.observable(null),
    currentAxes : ko.observableArray([]),
    loading : ko.observable(false),
    insights : ko.observableArray([{"CounterParty" : "SG", "Score": 177, rankingFirst : true}, {"CounterParty" : "BNP", "Score": 98, rankingSecond : true} , {"CounterParty" : "JP Morgan", "Score": 88, rankingThird : true}]),
    rankings : ko.observableArray([{"CounterParty" : "HSBC", "Score": "1st", rankingFirst : true}, {"CounterParty" : "BNP", "Score": "2nd", rankingSecond : true} , {"CounterParty" : "Crédit Suisse", "Score": "3rd", rankingThird : true}]),
    select_aggreg_axes : ko.observableArray([]),
    select_aggreg : function (data, event){
        if(viewModel.selected_aggreg())
            viewModel.selected_aggreg().selected(false);
        data.selected(true);
        viewModel.selected_aggreg(data);
        viewModel.select_aggreg_axes(data["Axes"]);
    }

};

viewModel.has_selected_aggreg = new ko.computed(function(){
    return viewModel.selected_aggreg()!= null;
}, viewModel);

ko.applyBindings(viewModel);

})();