function generateFinancialGoals(){
    // Mocked data for testing purposes
    var financialGoals = [
        { name: "Goal 1", description: "Description 1", type: "Income", targetAmount: 1000, currentAmount: 500 },
        { name: "Goal 2", description: "Description 2", type: "Income", targetAmount: 2000, currentAmount: 1500 },
        { name: "Goal 3", description: "Description 3", type: "Expense", targetAmount: 3000, currentAmount: 2800 },
        { name: "Goal 4", description: "Description 4", type: "Expense", targetAmount: 4000, currentAmount: 4000 },
        { name: "Goal 5", description: "Description 5", type: "Income", targetAmount: 5000, currentAmount: 5500 },
        { name: "Goal 6", description: "Description 6", type: "Expense", targetAmount: 6000, currentAmount: 4500 },
        { name: "Goal 7", description: "Description 7", type: "Income", targetAmount: 7000, currentAmount: 7000 },
        { name: "Goal 8", description: "Description 8", type: "Expense", targetAmount: 8000, currentAmount: 6000 },
        { name: "Goal 9", description: "Description 9", type: "Income", targetAmount: 9000, currentAmount: 9500 },
    ];

    // Function to create progress bar
    function createProgressBar(container, goal) {
        var progressPercentage = Math.round((goal.currentAmount / goal.targetAmount) * 100);

        var progressBar = document.createElement("div");
        progressBar.classList.add("goal-progress");
        progressBar.style.width = progressPercentage + "%";

        if (goal.type === "Income") {
            progressBar.classList.add(progressPercentage < 100 ? "income-red" : "income-green");
        } else if (goal.type === "Expense") {
            progressBar.classList.add(progressPercentage >= 100 ? "expense-red" : "expense-green");
        }

        var percentageLabel = document.createElement("div");
        percentageLabel.classList.add("goal-percentage");
        percentageLabel.textContent = progressPercentage + "%";

        container.appendChild(progressBar);
        container.appendChild(percentageLabel);
    }

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

        var goalProgressBarContainer = document.createElement("div");
        goalProgressBarContainer.classList.add("goal-progress-bar");

        var goalTips = document.createElement("div");
        goalTips.classList.add("goal-tips");

        if (goal.type === "Income") {
            if (goal.currentAmount < goal.targetAmount) {
                goalTips.textContent = "Earn more money";
            } else {
                goalTips.textContent = "You've earned enough, good job";
            }
        } else if (goal.type === "Expense") {
            if (goal.currentAmount < goal.targetAmount) {
                //goalTips.textContent = "Please, save money";
                goalTips.textContent = "You've saved enough, good job";
            } else {
                //goalTips.textContent = "You've saved enough, good job";
                goalTips.textContent = "Please, save money";
            }
        }

        goalContainer.appendChild(goalName);
        goalContainer.appendChild(goalType);
        goalContainer.appendChild(goalDescription);
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