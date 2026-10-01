// https://www.instagram.com/p/CLY5xRvh8MB/

const monthTemplate = () => /* html */`
    <div class="days-title"><div>ش</div><div>ی</div>
    <div>د</div><div>س</div><div>چ</div><div>پ</div>
    <div>ج</div></div><div class="days"></div>`;
const dpTemplate = () => /* html */`
  <div class="date-picker"><div class="head">
      <button><i class="material-icons">chevron_right</i></button>
      <div class='title'><h4>...</h4><div></div></div>
      <button><i class="material-icons">chevron_left</i></button>
    </div><div class="month"></div></div>`;

const calendar = document.querySelector('.calendar');
calendar.innerHTML = dpTemplate();
const today = moment().locale('fa');
const currentMonth = today.clone().date(1);
const vMonth = calendar.querySelector('.month');
const vTitle = calendar.querySelector('.head .title h4');
const vTitleEn = calendar.querySelector('.head .title div');
const manageMonth = (navigate = '') => {
    const month = document.createElement('div');
    month.innerHTML = monthTemplate();
    switch (navigate) {
        case 'next': currentMonth.add(1, 'months'); break;
        case 'prev': currentMonth.subtract(1, 'months'); break;
    }
    const fDay = currentMonth.clone();
    const lDay = fDay.clone().date(fDay.daysInMonth());
    vTitle.textContent = fDay.format('MMMM YYYY');
    vTitleEn.textContent = fDay.clone().locale('en').format('MMMM - ') +
        lDay.clone().locale('en').format('MMMM YYYY');
    const vDays = month.querySelector('.days');
    const addDay = (d, disabled = false) => {
        const vDay = document.createElement('div'),
            vDayEn = document.createElement('div');
        vDay.textContent = d.date();
        vDay.className = `${d.isSame(today, 'day') ? 'today' : ''} ` +
            `${disabled ? 'disable' : ''}`;
        vDayEn.textContent = d.clone().locale('en').date();
        vDay.appendChild(vDayEn);
        vDays.appendChild(vDay);
    }

    for (let d = fDay.clone().subtract((fDay.day() + 1) % 7, 'days');
        d.diff(fDay, 'days') <= -1; d.add(1, 'days')) {
        addDay(d, true)
    }

    for (let d = fDay.clone(); d.diff(lDay, 'days') <= 0; d.add(1, 'days')) {
        addDay(d)
    }
    for (let d = lDay.clone().add(1, 'days');
        d.diff(lDay.clone().add(7 - (lDay.day() + 1) % 7, 'days'), 'days') <= -1;
        d.add(1, 'days')) { addDay(d, true) }
    switch (navigate) {
        case 'next':
            vMonth.children[0].classList.add('right')
            month.classList.add('left'); break;
        case 'prev':
            vMonth.children[0].classList.add('left')
            month.classList.add('right'); break;
    }
    vMonth.appendChild(month);
    setTimeout(() => {
        month.className = '';
        vMonth.style.height = month.clientHeight + 'px';
    }, 20);
    if (navigate !== '') { setTimeout(() => vMonth.children[0].remove(), 500) }
}
manageMonth();
const btns = calendar.querySelectorAll('.head button');
btns[0].addEventListener('click', () => manageMonth('prev'))
btns[1].addEventListener('click', () => manageMonth('next'))
