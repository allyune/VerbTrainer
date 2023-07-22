var trainingInProgress = false;
var activeRow;
var startTrainingButton = document.querySelector('#btn-start');
var stopTrainingButton = document.querySelector('#btn-stop');

function getVerbConjugations(verbId) {
    return conjugationsData.filter(verb => verb.VerbId == verbId);
}

function loadConjugations() {
    var verbId = activeRow.getAttribute('verb-id');
    var verbConjugations = getVerbConjugations(verbId);
    var trainerContainer = document.getElementById('trainer-container');
    startTrainingButton.style.display = '';
    stopTrainingButton.style.display = 'none';
    trainerContainer.classList.remove('trainer-active');
    trainerContainer.classList.add('trainer-container-conjugations');
    trainerContainer.innerHTML = '';
    for (var conjugation of verbConjugations) {
        var conjDiv = document.createElement('div');
        conjDiv.classList.add('conj-div')
        //Meaning
        var conjMeaning = document.createElement('p');
        conjMeaning.innerHTML = conjugation.Meaning
        conjDiv.appendChild(conjMeaning);
        //Text
        var conjText = document.createElement('p');
        conjText.innerHTML = conjugation.Text
        conjDiv.appendChild(conjText);
        //Transcription
        var conjTranscription = document.createElement('p');
        conjTranscription.innerHTML = conjugation.Transcription
        conjDiv.appendChild(conjTranscription);
        trainerContainer.appendChild(conjDiv);
    }
}

function tableSearch(searchText) {
    var tableRows = document.getElementsByClassName('table-row');
    for (var row of tableRows) {
        var hebrewNameNikud = row.querySelector('.name').textContent.toLowerCase();
        var hebrewName = hebrewNameNikud.replace(/([\u05B0-\u05BD]|[\u05BF-\u05C7])/g, "");
        var englishMeaning = row.querySelector('.meaning').textContent.toLowerCase();
        if (
            searchText &&
            (!hebrewName.includes(searchText) && !englishMeaning.includes(searchText))
        ) {
            row.style.display = 'none';
        } else {
            row.style.display = '';
        }
    }
};

function styleActiveRow(row) {
    row.classList.add('row-clicked')
}

function trainVerb(conjId=0) {
    var verbId = activeRow.getAttribute('verb-id');
    var verbConjugations = getVerbConjugations(verbId);
    var trainerContainer = document.getElementById('trainer-container');
    startTrainingButton.style.display = 'none';
    stopTrainingButton.style.display = 'block';
    trainerContainer.classList.remove('trainer-container-conjugations');
    trainerContainer.classList.add('trainer-active');
    trainerContainer.innerHTML = '';
    trainerContainer.classList.add('trainer-active')
    var conjDiv = document.createElement('div');
    conjDiv.classList.add('training-conj-div')
    conjugation = verbConjugations[conjId];
    //Meaning
    var conjMeaning = document.createElement('p');
    conjMeaning.innerHTML = conjugation.Meaning
    conjDiv.appendChild(conjMeaning);
    //Text
    var conjText = document.createElement('p');
    conjText.innerHTML = conjugation.Text
    conjDiv.appendChild(conjText);
    //Transcription
    var conjTranscription = document.createElement('p');
    conjTranscription.innerHTML = conjugation.Transcription
    conjDiv.appendChild(conjTranscription);
    trainerContainer.appendChild(conjDiv);
    //Prev/Next
    var buttonsContainer = document.createElement('div');
    buttonsContainer.classList.add('training-buttons');
    trainerContainer.appendChild(buttonsContainer);
    var prevButton = document.createElement('button')
    prevButton.innerText = 'Prev';
    prevButton.classList.add('btn', 'btn-sm', 'btn-info', 'btn-purple');
    var nextButton = document.createElement('button')
    nextButton.innerText = 'Next';
    nextButton.classList.add('btn', 'btn-sm', 'btn-info', 'btn-purple');
    buttonsContainer.appendChild(prevButton);
    buttonsContainer.appendChild(nextButton);
    prevButton.addEventListener('click', function () {
        console.log(conjId)
        if (conjId > 0) {
            trainVerb(conjId - 1);
        } else {
            trainVerb(verbConjugations.length - 1);
        }
        
    })
    nextButton.addEventListener('click', function () {
        console.log(conjId)
        if (conjId < verbConjugations.length - 1) {
            trainVerb(conjId + 1);
        } else {
            trainVerb(0);
        }

    })
}

document.addEventListener('DOMContentLoaded', function () {
    activeRow = document.querySelector('.table-row')
    styleActiveRow(activeRow);
    loadConjugations();
    var searchInput = document.getElementById('verb-search');
    searchInput.addEventListener('input', function () {
        var searchText = this.value.toLowerCase();
        tableSearch(searchText)
    });

    var tableRows = document.querySelectorAll(".table-row");
    tableRows.forEach(row => {
        row.addEventListener('click', async function () {
            var verbId = this.getAttribute('verb-id');
            activeRow.classList.toggle('row-clicked');
            activeRow = this;
            styleActiveRow(this);
            try {
                if (!trainingInProgress) {
                    loadConjugations()
                } else {
                    trainVerb()
                }
                
            }
            catch (e) {
            console.log(e);
            }
        })
    })

startTrainingButton.addEventListener('click', function () {
    trainingInProgress = true;
    trainVerb();
});

stopTrainingButton.addEventListener('click', function () {
    trainingInProgress = false;
    loadConjugations();
});

});






