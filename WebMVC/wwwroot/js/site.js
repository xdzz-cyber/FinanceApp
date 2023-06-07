async function generateFinancialGoals(financialGoals) {
    console.log(financialGoals)
    // Mocked data for testing purposes
    // var financialGoals = [
    //     {
    //         name: "earn money for my family",
    //         description: "Kids and wife need more money",
    //         type: "Income",
    //         targetAmount: 1000,
    //         currentAmount: 500,
    //         budgetName: "Family",
    //     },
    //     {
    //         name: "earn money on charitable event to send it to kids",
    //         description: "gotta earn money to help kids",
    //         type: "Income",
    //         targetAmount: 2000,
    //         currentAmount: 1500,
    //         budgetName: "Leisure",
    //     },
    // ];

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

    // Function to get advice for financial goal
    async function getAdvice(goals) {
        let prompt = "";
        goals.forEach((goal, idx) => {
            const { name, description, type, targetAmount, currentAmount, budgetName } = goal;
            prompt += `I want you to generate me a meaningful advice (64 symbols at max) for each financial goal (there are ${goals.length} of them and separate answers for each by ; symbol), and here's the data for the current goal which is the ${idx + 1}th one (name: ${name}, description: ${description}, type: ${type}, targetAmount: ${targetAmount}, currentAmount: ${currentAmount}). Type "income" means that you need to earn more money, type "expense" means that you need to save more money. Budget name is ${budgetName};`;
        });

        const headers = {
            "Content-Type": "application/json",
            "Authorization": "Bearer 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"
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