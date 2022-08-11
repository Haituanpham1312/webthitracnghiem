// JavaScript source code


const data = [
    {
        id: 1,
        answer: ['Ag', 'Cu', "Al", "Mg"],
        question: "Chất nào dẫn nhiệt tốt nhất?",
        correct: 'Ag'
    },
    {
        id: 2,
        answer: ['Xe ô tô', 'Cột đèn bên đường', "Bóng đèn trên xe", "Hành khách đang ngồi trên xe"],
        question: "Một người ngồi trên xe đi từ TP HCM ra Đà Nẵng, nếu lấy vật làm mốc là tài xế đang lái xe thì vật chuyển động là ?",
        correct: 'Cột đèn bên đường'
    },
    {
        id: 3,
        answer: ['6 phút 15,16 giây.', '7 phút 16,21 giây.', "10 phút 12,56 giây.", "12 phút 16,36 giây."],
        question: "Nếu lấy mốc  thời gian là lúc 5 giờ 15 phút thì sau ít nhất bao lâu kim phút đuổi kịp kim giờ?",
        correct: '10 phút 12,56 giây.'
    },
     {
        id: 4,
         answer: ['6km', '-6km', "4km", "-4km"],
         question: "	Phương trình chuyển động của một chất điểm dọc theo trục Ox có dạng: x = 2t - 10 (km, giờ). Quãng đường đi được của chất điểm sau 3h là:",
        correct: '12 phút 16,36 giây.'
    },
      {
        id: 5,
        answer: ['4m.', '3m.', "2m.", "1m."],
          question: "Một chiếc bắt đầu tăng tốc từ nghỉ với gia tốc 2 m/s2. Quãng đường xe chạy được trong giây thứ hai là?",
        correct: '4m.'
    },
       {
        id: 6,
           answer: ['0.05s', '0.45s', "0.5s", "0.25s"],
           question: "Một vật rơi thẳng đứng từ độ cao 19,6 m với vận tốc ban đầu bang 0 (bỏ qua sức cản không khí, lấy g = 9,8 m/s2). Thời gian vật đi được 1 m cuối cùng bằng?",
        correct: '0.45s.'
    },
    {
        id: 7,
        answer: ['Hòn bi lăn trên mặt bàn ngang.', 'Pitong lên xuống trong ống bơm xe.', " Kim đồng hồ đang chạy.", "0 Cánh quạt máy đang quay."],
        question: "Chuyển động nào sau đây là chuyển động tịnh tiến:",
        correct: 'Hòn bi lăn trên mặt bàn ngang..'
    },
    {
        id: 8,
        answer: ['. Đứng yên.', 'Chạy lùi về phía sau.', "Tiến về phía trước.", "Tiến về phía trước rồi sau đó lùi về phía sau."],
        question: "Hành khách trên tàu A thấy tàu B đang chuyển động về phía trước. Còn hành khách trên tàu B lại thấy tàu C cũng đang chuyển động về phía trước. Vậy hành khách trên tàu A sẽ thấy tàu C:",
        correct: 'Tiến về phía trước rồi sau đó lùi về phía sau..'
    },
    {
        id: 9,
        answer: ['604 m/s.', '605 m/s.', "605 m/s.", "607 m/s."],
        question: "Trái đất quanh quanh trục Bắc - Nam với chuyển động đều mỗi vòng 24 giờ. Bán kính Trái Đất R = 6400 km. Tại một điểm trên mặt đất có vĩ độ = 30o có tốc đô dài bằng",
        correct: '604 m/s.'
    },
    {
        id: 10,
        answer: [' 8π (s).', '6π (s).', " 12π (s).", "10π (s)."],
        question: "Một vật chuyển động theo đường tròn bán kính r = 100 cm với gia tốc hướng tâm an = 4 cm/s2. Chu kì T của chuyển động vật đó là??",
        correct: '10π (s)..'
    },
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


handleRandomQuestion(10)


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
            console.log(6)
        }
        else {
            question[arr.index].innerHTML = 'Correct'
        }
    })


}