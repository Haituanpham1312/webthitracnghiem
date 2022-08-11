const data = [
    {
        id: 1,
        answer: ['20182330', '20182331', "20182332", "20182333"],
        question: "MSSV của gutboiz Thái Bình là gì ?",
        correct: '20182330'
    },
    {
        id: 2,
        answer: ['20182486', '20182487', "20182488", "20182489"],
        question: "MSSV của fukboiz Hải Dương là gì ?",
        correct: '20182486'
    },
    {
        id: 3,
        answer: ['Đức Anh', 'sdsdvdsvds'],
        question: "Trang giấy trắng là ai ?",
        correct: 'Đức Anh'
    }
]

let list = []

const handleRandomQuestion = (number) => {
    for (let i = 1; i <= number; i++) {
        let index = Math.round(Math.random() * (data.length - 1))
        list = [...list, ...[...data.splice(index, 1)]];
    }
    handleRandomAnser(list)
    handleShowQuestion(list)
}

const handleRandomAnser = (list) => {
    list.forEach(arr => {
        arr.answer.sort(() => Math.random() - 0.5)

    });
    console.log(list)
}
let number = 1;
let questionIndex = 0;
let AnswerIndex = 0;
let root = document.querySelector('.root')
const handleShowQuestion = (list) => {
    let html = ``

    list.map(arr => {
        html +=
            `
        <div class="question${arr.id}">
            <b>Câu ${number} :</b> ${arr.question}  <span class="pl-5 rs"></span>
            <div>
       ${arr.answer.map((answer, index) => {
                return ` <input type="radio" class="pl-5 ml-5" name="Answer${number}" value="${answer}" questionId=${arr.id} answerIndex=${index} questionIndex=${questionIndex} id=""> ${answer} <span class="answer__result${index} questionIndex${questionIndex}"> </span>`
            }).join(" ")}
            </div>
        </div>
        `
        number++;
        questionIndex++;
    }).join(" ")
    root.innerHTML = html
}

const handleGetAnswer = () => {

    let arrAnswer = []
    let input = document.querySelectorAll("input[type=radio]:checked")
    input.forEach(arr => arrAnswer = [...arrAnswer, { answer: arr.value, id: arr.attributes.questionId.value, index: arr.attributes.questionIndex.value, indexAnswer: arr.attributes.answerIndex.value }])

    check(arrAnswer, list)
}


handleRandomQuestion(2)


const check = (arrAnswer, list) => {
    let number = document.querySelector('.number')
    let numberCorrect = 0;
    let arrWrong = [];
    let arrIndexAnswerTrue = [];
    list.forEach((arrs, index) => {
        let indexAnswer = arrs.answer.findIndex(answer => answer == arrs.correct)
        arrIndexAnswerTrue.push({ indexTrue: indexAnswer, index })
    })
    console.log(arrIndexAnswerTrue)
    arrAnswer.forEach(arr => {

        let result = list.some(list => list.id == arr.id && list.correct == arr.answer)
        if (result) {
            numberCorrect += 1;

        }
        else {
            arrWrong.push({ id: arr.id, index: arr.index, answer: arr.answer, indexAnswer: arr.indexAnswer, result: false })
        }
        console.log(arrWrong)
        number.innerHTML = numberCorrect
    })
    handleShowTrueAnswer(arrIndexAnswerTrue)
    handleShowWrongQuestion(arrWrong)
}

const handleShowTrueAnswer = (arrTrue) => {
    let question = document.querySelectorAll('.rs')

    arrTrue.forEach(arr => {
        let answer__result = document.querySelector(" .questionIndex" + arr.index + ".answer__result" + arr.indexTrue)
        answer__result.innerHTML = `<i class="fa-solid fa-check text-success"></i>`
        if (arr.result == false) {
            question[arr.index].innerHTML = 'Wrong'
            question[arr.index].classList.add = 'text-danger'
            question[arr.index].classList.remove('text-success')
            console.log(1)
        }
        else {
            question[arr.index].innerHTML = 'Correct'
            question[arr.index].classList.add('text-success')
        }
        // Theem class

    })
}

const handleShowWrongQuestion = (arrWrong) => {
    let question = document.querySelectorAll('.rs')

    arrWrong.forEach(arr => {
        let answer__result = document.querySelector(" .questionIndex" + arr.index + ".answer__result" + arr.indexAnswer)
        answer__result.innerHTML = `<i class="text-danger fa-solid fa-xmark"></i>`

        if (arr.result == false) {
            question[arr.index].innerHTML = 'Wrong'
            question[arr.index].classList.remove('text-success')
            question[arr.index].classList.add('text-danger')
            console.log(2)
        }
        else {
            question[arr.index].innerHTML = 'Correct'
        }
    })


}