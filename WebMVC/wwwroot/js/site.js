﻿const storageNameOfCoinsIds = "coinsIds";


async function generateFinancialGoals(financialGoals) {
    function createProgressBar(container, goal) {
        var progressPercentage = Math.round((goal.currentAmount / goal.targetAmount) * 100);
        var oldProgressPercentage = progressPercentage;
        var progressBar = document.createElement("div");
        progressBar.classList.add("goal-progress");
        progressPercentage = progressPercentage > 100 ? 105 : progressPercentage;
        progressBar.style.width = progressPercentage + "%";

        if (goal.type === "Income") {
            progressBar.classList.add(progressPercentage < 100 ? "income-red" : "income-green");
        } else if (goal.type === "Expense") {
            progressBar.classList.add(progressPercentage >= 100 ? "expense-red" : "expense-green");
        }

        var percentageLabel = document.createElement("div");
        percentageLabel.classList.add("goal-percentage");
        percentageLabel.textContent = oldProgressPercentage + "%";

        container.appendChild(progressBar);
        container.appendChild(percentageLabel);
    }

    // Function to get advice for financial goal
    async function getAdvice(goals) {
        let prompt = "";
        goals.forEach((goal, idx) => {
            const { name, description, type, targetAmount, currentAmount, budgetName } = goal;
            prompt += `I want you to generate me a meaningful advice (64 symbols at max) for each financial goal (there are ${goals.length} of them and separate answers for each by ; symbol), and here's the data for the current goal which is the ${idx + 1}th one (name: ${name}, description: ${description}, type: ${type}, targetAmount: ${targetAmount}, currentAmount: ${currentAmount}). Type "income" means that you need to earn more money, type "expense" means that you need to save more money. Budget name is ${budgetName};`;
        });

        const headers = {
            "Content-Type": "application/json",
            "Authorization": "Bearer "
        };

        const data = {
            "max_tokens": 128,
            "temperature": 0.6,
            "model": "gpt-3.5-turbo",
            "n": 1,
            "messages": [
                {
                    "role": "user",
                    "content": prompt
                }
            ]
        };

        const response = await fetch("https://api.openai.com/v1/chat/completions", {
            method: "POST",
            headers: headers,
            body: JSON.stringify(data)
        });
        const responseData = await response.json();
        console.log(responseData.choices)
        return responseData.choices;
    }

    const adviceChoices = await getAdvice(financialGoals);

    // Generate financial goals
    var container = document.getElementById("financial-goals-container");

    var rowContainer;
    financialGoals.forEach(function (goal, index) {
        if (index % 3 === 0) {
            rowContainer = document.createElement("div");
            rowContainer.classList.add("row-container");
            container.appendChild(rowContainer);
        }

        var goalContainer = document.createElement("div");
        goalContainer.classList.add("goal-container", goal.type.toLowerCase());

        var goalName = document.createElement("div");
        goalName.classList.add("goal-name");
        goalName.textContent = goal.name;

        var goalType = document.createElement("div");
        goalType.classList.add("goal-type");
        goalType.textContent = goal.type;

        var goalDescription = document.createElement("div");
        goalDescription.classList.add("goal-description");
        goalDescription.textContent = goal.description;

        var budgetName = document.createElement("div");
        budgetName.classList.add("budget-name");
        budgetName.textContent = "Budget: " + goal.budgetName;

        var goalProgressBarContainer = document.createElement("div");
        goalProgressBarContainer.classList.add("goal-progress-bar");

        var goalTips = document.createElement("div");
        goalTips.classList.add("goal-tips");
        const splitCharacter = adviceChoices[0].message.content.includes(";") ? ";" : "\n";
        console.log(adviceChoices[0].message.content)
        console.log(adviceChoices[0].message.content.split(splitCharacter))
        let tips = adviceChoices[0].message.content.split(splitCharacter);
        tips = tips.filter(function (el) {
            return el !== "";
        });
        console.log(tips[0])
        console.log(tips[1])
        goalTips.textContent = tips[index];

        goalContainer.appendChild(goalName);
        goalContainer.appendChild(goalType);
        goalContainer.appendChild(goalDescription);
        goalContainer.appendChild(budgetName);
        goalContainer.appendChild(goalProgressBarContainer);
        goalContainer.appendChild(goalTips);

        rowContainer.appendChild(goalContainer);

        createProgressBar(goalProgressBarContainer, goal);
    });
}
function generateTransactions(transactionsJson){
    //let transactionsJson = '@Html.Raw(Model.TransactionsJson)';
    let transactions = JSON.parse(transactionsJson);

    let incomeData = {
        type: 'scatter',
        x: [],
        y: [],
        mode: 'lines',
        name: 'Income',
        line: {
            color: 'rgb(219, 64, 82)',
            width: 3
        }
    };

    let expenseData = {
        type: 'scatter',
        x: [],
        y: [],
        mode: 'lines',
        name: 'Expense',
        line: {
            color: 'rgb(55, 128, 191)',
            width: 1
        }
    };

    // Sort transactions by date and by amount
    transactions.sort(function (a, b) {
        return new Date(a.Date) - new Date(b.Date) || a.Amount - b.Amount;
    });

    transactions.forEach(function (transaction) {
        // Check if transaction is income or expense
        if (transaction.CategoryName === "Income") {
            incomeData.x.push(transaction.Date);
            incomeData.y.push(transaction.Amount);
        }
        else {
            expenseData.x.push(transaction.Date);
            expenseData.y.push(transaction.Amount);
        }
    });

    var layout = {
        width: 650,
        height: 650
    };

    var data = [incomeData, expenseData];

    Plotly.newPlot('incomeExpenseChart', data, layout);
}

function generateBarDiagramPerCoin(coins){
    let connection = new signalR.HubConnectionBuilder().withUrl("/coinsHub").build();

    let latestPrices = coins.reduce((obj, coin) => {
        obj[coin] = null;
        return obj;
    }, {});

    // Object to store historical data for each coin
    let coinData = coins.reduce((obj, coin) => {
        obj[coin] = {
            prices: [], // Array to store historical prices
            maxBars: 7, // Maximum number of bars to display
        };
        return obj;
    }, {});

    // Chart.js configuration
    let chartOptions = {
        responsive: true,
        scales: {
            y: {
                beginAtZero: true,
                ticks: {
                    precision: 0,
                    stepSize: 0.001, // Increment by 1 unit
                    maxTicksLimit: 10, // Maximum number of ticks (labels) to display
                },
            },
        },
    };

    let coinCharts = coins.reduce((obj, coin) => {
        let canvasId = `chart-${coin}`;
        let canvasElement = document.getElementById(canvasId);
        let ctx = canvasElement.getContext("2d");
        let chart = new Chart(ctx, {
            type: "bar",
            data: {
                labels: [],
                datasets: [{
                    label: coin,
                    data: [],
                    backgroundColor: "rgba(75, 192, 192, 0.5)",
                    borderColor: "rgba(75, 192, 192, 1)",
                    borderWidth: 1,
                    barThickness: 5, // Adjust the thickness of bars
                    maxBarThickness: 10, // Adjust the maximum thickness of bars
                    minBarLength: 2,
                }],
            },
            options: chartOptions,
        });
        obj[coin] = chart;
        return obj;
    }, {});


    connection.start()
        .then(function () {
            console.log("SignalR connection established.")
        })
        .catch(function (err) {
            console.error(err.toString());
        });

    setInterval(function () {
        connection.invoke("UpdateCoinPrices", coins)
            .catch(function (error) {
                console.error("Error updating coin prices:", error);
            });
    }, 2000); // Update every two seconds

    connection.on("ReceiveCoinPrices", function (prices) {
        // Update coin prices in the HTML
        const data = JSON.parse(prices).data;
        // Update coin prices in the HTML
        console.log("inside ReceiveCoinPrices", data);
        data.forEach(function (coin) {
            //const priceElement = document.getElementById("price-" + coin.name.toLowerCase());
            const chartContainer = document.getElementById("chart-container-" + coin.name.toLowerCase());
            let priceElement = document.getElementById("price-" + coin.name.toLowerCase());
            if (priceElement && chartContainer) {
                priceElement.textContent = `Price: ${Number(coin.priceUsd).toPrecision(6)} USD`;
            }
            let coinHistory = coinData[coin.name.toLowerCase()];

            if (coin.priceUsd !== latestPrices[coin.name.toLowerCase()]) { // !==
                // Calculate the difference between the current price and the previous price
                const currentPrice = Number(coin.priceUsd).toPrecision(6);
                debugger
                
                
                // Limit the historical data to the maximum number of bars
                if (coinHistory.prices.length >= coinHistory.maxBars) {
                    coinHistory.prices.shift(); // Remove the oldest price
                }

                // Add the new price to the historical data
                coinHistory.prices.push(currentPrice);
                
                // Update the latest price
                latestPrices[coin.name.toLowerCase()] = coin.priceUsd;

                // Update the chart data and labels
                let chart = coinCharts[coin.name.toLowerCase()];
                chart.data.labels = coinHistory.prices.map((_, index) => `Price ${index + 1}`);

                // Adjust the bar heights based on the price difference
                //let c = 0;
                chart.data.datasets[0].data = coinHistory.prices.map((price, index) => {
                    if (index === 0) { //&& coinHistory.prices.length === 0 && index === 0
                        //c++;
                        return price * 0.01;
                    } else {
                        // const previousPrice = index === coinHistory.prices.length - 2 ? coinHistory.prices[index - 1]
                        //     : coinHistory.prices[coinHistory.prices.length - index - 1];
                        const previousPrice = coinHistory.prices[index - 1];
                        return (price - previousPrice) * 100;
                    }
                    //c++;
                });

                // Update the chart
                chart.update();
            }
        });
    });
}

function renderCards(items) {
    console.log("inside renderCards", items)
    // Clear previous cards
    const cardContainer1 = document.getElementById('cardContainer1');
    const cardContainer2 = document.getElementById('cardContainer2');
    cardContainer1.innerHTML = '';
    cardContainer2.innerHTML = '';

    // Render new cards
    items.accounts.forEach(account => {
        const card = createCard(account, type='account', name=items.name);
        cardContainer1.appendChild(card);
    });

    items.jars.forEach(jar => {
        const card = createCard(jar);
        cardContainer2.appendChild(card);
    });
}

function createCard(item, type='jar', name='') {
    const card = document.createElement('div');
    card.className = 'card';
    card.style.width = '18rem';

    const i = document.createElement('i');
    i.className = type !== 'jar' ? 'card-img-overlay fa-solid fa-credit-card fa-2xl' : 'card-img-overlay fa-solid fa-piggy-bank fa-2xl'; 
    card.appendChild(i);

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body';
    card.appendChild(cardBody);

    const title = document.createElement('h4');
    title.className = 'card-title text-center';
    title.textContent = item.title || name;
    cardBody.appendChild(title);
    
    const stripeId = document.createElement('h5');
    stripeId.className = 'card-title text-center';
    stripeId.textContent = `StripeId: ${item.stripeId}` ; // Set the stripeId here
    cardBody.appendChild(stripeId);

    const description = document.createElement('p');
    description.className = 'card-text';
    //description.textContent = item.description || ''; // Set the description here
    description.textContent = type !== 'jar' ? `Credit limit: ${item.creditLimit} .Balance: ${item.balance}$, type: ${item.type} and cashback type ${item.cashbackType}` 
        : `Description: ${item.description}. Goal: ${item.goal}. Balance: ${item.balance}$`;
    cardBody.appendChild(description);

    // const link = document.createElement('a');
    // link.href = '#';
    // link.className = 'btn btn-primary';
    // link.textContent = 'Go somewhere';
    // cardBody.appendChild(link);

    return card;
}

function subscribeForBankingInfo(){
    let connection = new signalR.HubConnectionBuilder().withUrl("/bankingHub").build();

    connection.start()
        .then(function () {
            console.log("SignalR connection to banking hub established.")
        })
        .catch(function (err) {
            console.error(err.toString());
        });

    // setInterval(function () {
    //     connection.invoke("UpdateBankingInfo")
    //         .catch(function (error) {
    //             console.error("Error updating coin prices:", error);
    //         });
    // }, 60000);

    connection.on("ReceiveBankingInfo", function (items) {
        console.log(`Received banking info: ${items}`, JSON.parse(items));
        renderCards(JSON.parse(items));
    });
}

function getBankingInfo() {
    // $(document).ready(function () {
    //     $.ajax({
    //         url: '@Url.Action("ExecuteBackgroundJob", "Banking")',
    //         type: 'POST',
    //         success: function () {
    //             console.log('Background job executed successfully');
    //         },
    //         error: function () {
    //             console.error('An error occurred while executing the background job');
    //         }
    //     });
    // });

    document.addEventListener("DOMContentLoaded", function() {
        // Make request to the server via fetch API
        fetch('/Banking/ExecuteBackgroundJob/', {
            method: 'POST'
        }).then(function (response) {
            console.log('Background job executed successfully');
        }).catch(function (error) {
            console.error('An error occurred while executing the background job');
        });
    });
}

function addCoinsToCart(){
    const maxNumberOfRecipesAllowedToStore = 10;
    const gridColsParentOfCards = document.querySelector("#gridColsParentOfCards");
    
    gridColsParentOfCards?.addEventListener("change", function(e) {
        let currentElement = e.target
        let elementWithRecipeIdValue = currentElement.nextElementSibling
        let coinsIds = JSON.parse(localStorage.getItem(storageNameOfCoinsIds))
        console.log(currentElement, elementWithRecipeIdValue,elementWithRecipeIdValue.value, currentElement.checked)

        if(currentElement.checked){
            coinsIds.push(elementWithRecipeIdValue.value)
        } else{
            coinsIds = coinsIds.filter(id => id !== elementWithRecipeIdValue.value)
        }
        localStorage.setItem(storageNameOfCoinsIds, JSON.stringify(coinsIds))
    });

    function init(){
        if(!localStorage.getItem(storageNameOfCoinsIds)
            || JSON.parse(localStorage.getItem(storageNameOfCoinsIds)).length > maxNumberOfRecipesAllowedToStore){
            localStorage.setItem(storageNameOfCoinsIds, JSON.stringify([]))
        }
    }

    // function clearStorage(){
    //     // setInterval(function () {
    //     //     localStorage.clear()
    //     // }, 1000 * 60 * 5)
    //     localStorage.setItem(storageNameOfCoinsIds, JSON.stringify([]))
    // }

    init()
}

function addAllRecipesMarkedAsSaved(){
    console.log(`in addAllRecipesMarkedAsSaved`, localStorage.getItem(storageNameOfCoinsIds), localStorage);
    fetch("https://localhost:7069/Cart/AddToCart/", {
        headers: {
            "Accept":"application/json",
            "Content-Type": "application/json"
        },
        
        body: JSON.stringify(localStorage.getItem(storageNameOfCoinsIds)),
        method: "POST"
    }).then(r => r.text()
        .then(t => {
            clearStorage()
            window.location = `https://localhost:7214/Home/ShowResult?${new URLSearchParams({result:t}).toString()}`
        }))
}

function clearStorage(){
    localStorage.setItem(storageNameOfCoinsIds, JSON.stringify([]))
}

function setListenerForBudgetIdInCartPage(){
    var dropdown = document.querySelector('#SelectedBudgetId');
    var hiddenInput = document.querySelector('#selectedBudgetId');
    console.log('dropdown', dropdown, 'hiddenInput', hiddenInput)
    // Handle the change event of the dropdown
    dropdown.addEventListener('change', function() {
        console.log('dropdown.value', dropdown.value)
         // Get the selected value from the dropdown
        hiddenInput.value = dropdown.value; // Update the hidden input field's value
        console.log('hiddenInput.value', hiddenInput.value)
    });
}

function proceedTransaction(stripePublicKey){
    console.log('key', stripePublicKey)
    //var stripe = Stripe('@ViewBag.StripePublicKey');
    var stripe = Stripe(stripePublicKey);
    var elements = stripe.elements();
    
    var card = elements.create('card');
    card.mount('#card-element');

    var form = document.getElementById('payment-form');

    form.addEventListener('submit', function (event) {
        event.preventDefault();

        stripe.createToken(card).then(function (result) {
            if (result.error) {
                // Display error message to the user
                console.error(`error: ${result.error.message}`);
            } else {
                // Token created successfully, submit the form with the token
                console.log(`success: ${result.token}`)
                var cardNumber = result.token.card.number;
                console.log('cardNumber:', cardNumber);
                stripeTokenHandler(result.token);
                //console.log(`result.token: ${result.token}`, 'result', result)
            }
        });
    });

    function stripeTokenHandler(token) {
        // Insert the token ID into the form so it gets submitted to the server
        console.log(`token: ${token}`)
        var hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'stripeToken');
        hiddenInput.setAttribute('value', token.id);
        form.appendChild(hiddenInput);
        
        
        // var cardNumberElement = document.querySelector('input[name="cardnumber"]');
        // var cardNumberValue = cardNumberElement.value.replace(/\s/g, ''); // Remove whitespace from the value
        // console.log('cardNumber', cardNumberElement)
        // var hiddenCardNumberInput = document.createElement('input');
        // hiddenCardNumberInput.setAttribute('type', 'hidden');
        // hiddenCardNumberInput.setAttribute('name', 'cardNumber');
        // hiddenCardNumberInput.setAttribute('value', cardNumberValue);
        // form.appendChild(hiddenCardNumberInput);
        //
        // // Submit the form
         form.submit();
    }
}