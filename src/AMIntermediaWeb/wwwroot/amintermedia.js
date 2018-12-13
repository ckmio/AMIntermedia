(function (){


    function findOrder(Id)
    {
        for(var i=0, n=viewModel.aggregations().length; i<n; i++)
        {
            var currentval = viewModel.aggregations()[i];
            if(currentval.Id == Id) return currentval;
        }
        return null;
    }

function handleMessage(strMessage){
    if(strMessage.startsWith("order-update:")){
        var newAggreg = JSON.parse(strMessage.substring("order-update:".length));
        var order = findOrder(newAggreg.Id); 
        if(!order) return;
        var selected = order.selected();
        viewModel.aggregations.remove(order);
        viewModel.aggregations.unshift(newAggreg);
        if(!selected){ newAggreg.selected = ko.observable(false); return;}
        newAggreg.selected = ko.observable(true);
        viewModel.selected_aggreg(newAggreg);
        viewModel.select_aggreg_axes(newAggreg.Axes);
        return;
    }

    if(strMessage.startsWith("order-add:")){
        var newAggreg = JSON.parse(strMessage.substring("order-add:".length));
        newAggreg.selected = ko.observable(false);
        viewModel.aggregations.push(newAggreg);
        return;
    }

    if(strMessage.startsWith("order-delete:")){
        var newAggreg = JSON.parse(strMessage.substring("order-delete:".length));
        newAggreg.selected = ko.observable(false);
        viewModel.aggregations.push(newAggreg);
        return;
    }

    if(strMessage.startsWith("axe-update:")){
        var newAggreg = JSON.parse(strMessage.substring("axe-update:".length));
       console.log("received new axes");
        return;
    }

    if(strMessage.startsWith("init:")){
        var allAggregations = JSON.parse(strMessage.substring("init:".length));
        for(var  i=0; i< allAggregations.length; i++){
            var aggI = allAggregations[i];
            aggI.selected = ko.observable(false);
            viewModel.aggregations.push(aggI);
        }
        return;
    }

    if(strMessage.startsWith("performance-rankings:")){
        var newAggreg = JSON.parse(strMessage.substring("performance-rankings:".length));
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
    s.send("a new client is connected");
}



var viewModel = {
    aggregations : ko.observableArray([]),
    selected_aggreg : ko.observable(null),
    currentAxes : ko.observableArray([]),
    loading : ko.observable(false),
    insights : ko.observableArray([{"CounterParty" : "SG", "Score": 177, rankingFirst : true}, {"CounterParty" : "BNP", "Score": 98, rankingSecond : true} , {"CounterParty" : "JP Morgan", "Score": 88, rankingThird : true}]),
    rankings : ko.observableArray([{"CounterParty" : "HSBC", "Score": "1st", rankingFirst : true, vol : '100K', mks : '8%'}, {"CounterParty" : "BNP", "Score": "2nd", rankingSecond : true,vol : '90K', mks : '7.2%'} , {"CounterParty" : "Crédit Suisse", "Score": "3rd", rankingThird : true, vol : '80K', mks : '6.4%'}]),
    select_aggreg_axes : ko.observableArray([]),
    current_sector : ko.observable({
        name : 'Telecoms',
        demand : 2000000,
        supply : 3000000,
        net : 1000000
    }),
    select_aggreg : function (data, event){
        if(viewModel.selected_aggreg())
            viewModel.selected_aggreg().selected(false);
        data.selected(true);
        viewModel.selected_aggreg(data);
        viewModel.select_aggreg_axes(data["Axes"]);
    }

};

viewModel.aggregations.extend({ rateLimit: { timeout: 500, method: "notifyWhenChangesStop" } });

viewModel.has_selected_aggreg = new ko.computed(function(){
    return viewModel.selected_aggreg()!= null;
}, viewModel);


viewModel.selected_aggreg_supply = new ko.computed(function(){
    return viewModel.selected_aggreg()!= null;
}, viewModel);

viewModel.selected_aggreg_demand =  new ko.computed(function(){
    return viewModel.selected_aggreg()!= null;
}, viewModel);

viewModel.selected_aggreg_net =  new ko.computed(function(){
    return viewModel.selected_aggreg()!= null;
}, viewModel);

ko.applyBindings(viewModel);

})();