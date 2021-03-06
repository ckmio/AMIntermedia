(function (){


    var global_dpr_and_insights = {
        'GOVIES':[
            {"CounterParty" : "SG", "Rank":"1st", "Score": "-", rankingFirst : true,  Mode : "LT", Method : "Auto", Venue : "TW" ,vol : '2.8 Bn', mks : '23%'},
            {"CounterParty" : "BNP", "Rank":"2nd","Score": "-", rankingSecond : true, Mode : "LT", Method : "Auto", Venue : "TW" ,vol : '2.0 Bn', mks : '12.3%'} ,
            {"CounterParty" : "MLYNCH", "Score": "-","Rank":"3rd", rankingThird : true, Mode : "LT", Method : "Auto", Venue : "TW" ,vol : '1.5 Bn', mks : '8.8%'}
        ],
        'CORP':[
            {"CounterParty" : "MLYNCH","Rank":"1st", "Score": "-", rankingFirst : true,  Mode : "LT", Method : "RFQ", Venue : "TW" ,vol : '1.10 Bn', mks : '7.9%'},
            {"CounterParty" : "BNP", "Score": "-","Rank":"2nd", rankingSecond : true, Mode : "LT", Method : "RFQ", Venue : "TW" ,vol : '1.06 Bn', mks : '7.4%'} ,
            {"CounterParty" : "MS", "Score": "-","Rank":"3rd", rankingThird : true, Mode : "LT", Method : "RFQ", Venue : "TW" ,vol : '0.93 Bn', mks : '6.9%'}
        ],
        'EMERGING':[
            {"CounterParty" : "JPM","Rank":"1st", "Score": "-", rankingFirst : true,  Mode : "HT", Method : "Voice", Venue : "Voice" ,vol : '0.92 Bn', mks : '15.7%'},
            {"CounterParty" : "HSBC", "Score": "-","Rank":"2nd", rankingSecond : true, Mode : "HT", Method : "Voice", Venue : "Voice" ,vol : '0.80 Bn', mks : '13.6%'} ,
            {"CounterParty" : "BNP", "Score": "-", "Rank":"3rd",rankingThird : true, Mode : "HT", Method : "Voice", Venue : "Voice" ,vol : '0.76 Bn', mks : '12.9%'}
        ]
    };
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
    var agg =  viewModel.selected_aggreg();
    var axes = viewModel.select_aggreg_axes()
    if(!agg) return 'Global Supply : --'
    var sum =0;
    for(var i =0; i<axes.length; i++)
    {
        var axe = axes[i];
        if(axe.BuyOrSell=='SELL')
            sum += axe.Amount;
    }
    return 'Global Supply : '+sum;
    
}, viewModel);

viewModel.selected_aggreg_demand =  new ko.computed(function(){
    var agg =  viewModel.selected_aggreg();
    var axes = viewModel.select_aggreg_axes()
    if(!agg) return 'Global Demand : --'
    var sum =0;
    for(var i =0; i<axes.length; i++)
    {
        var axe = axes[i];
        if(axe.BuyOrSell=='BUY')
            sum += axe.Amount;
    }
    return 'Global Demand : '+sum;
}, viewModel);

viewModel.selected_aggreg_net =  new ko.computed(function(){
    var agg =  viewModel.selected_aggreg();
    var axes = viewModel.select_aggreg_axes()
    if(!agg) return 'Net Demand : --'
    var sum =0;
    for(var i =0; i<axes.length; i++)
    {
        var axe = axes[i];
        if(axe.BuyOrSell=='BUY')
            sum += axe.Amount;
        else{
            sum -= axe.Amount;
        }
    }
    return sum>0? 'Net Supply : '+sum : 'Net Demand : '+sum;
}, viewModel);

ko.applyBindings(viewModel);

})();