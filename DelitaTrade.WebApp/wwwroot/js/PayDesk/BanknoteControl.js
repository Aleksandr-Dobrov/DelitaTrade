document.addEventListener('DOMContentLoaded', addEvents);
function addEvents() {
    const totalSumEl = document.querySelector('#sum');
    const differenceEl = document.querySelector('#dif');
    const income = parseFloat(document.querySelector('#income').innerHTML);
    document.querySelectorAll('.bank').forEach((d) => {
        const countEl = d.querySelector('.count input');
        const up = d.querySelector('.up');
        up.addEventListener('click', (e) => {
            e.preventDefault();
            const el = d.querySelector('.value');
            el.value++;
        });
        const down = d.querySelector('.down');
        down.addEventListener('click', (e) => {
            e.preventDefault();
            const el = d.querySelector('.value');
            if (el.value > (countEl.value * -1)) {
                el.value--;
            }
        });

        d.addEventListener('wheel', (e) => {
            const el = d.querySelector('.value');
            if (e.deltaY > 0) {
                e.preventDefault();
                if (el.value > (countEl.value * -1)) {
                    el.value--;
                }
            }
            else if (e.deltaY < 0) {
                e.preventDefault();
                el.value++;
            }
        });
                
        const value = d.querySelector('.value');

        d.addEventListener('wheel', calculateTotal);
        value.addEventListener('input', calculateTotal);
        d.querySelectorAll('.banknote-command button').forEach((b) => b.addEventListener('click', calculateTotal));

        function calculateTotal(e) {
            const count = d.querySelector('.count input');
            const total = d.querySelector('.total input');
            const banknote = d.querySelector('.amount input');

            total.value = ((parseFloat(count.value) + parseFloat(value.value)) * parseFloat(banknote.value)).toFixed(2);
            calculateTotalSumAndDifference(totalSumEl, differenceEl, income);
        };
    });
}  

function calculateTotalSumAndDifference(totalSumEl, differenceEl, income) {
    const banknoteTotals = document.querySelectorAll('.total input');
    const totalSum = Array.from(banknoteTotals)
        .map(b => parseFloat(b.value) || 0)
        .reduce((acc, val) => acc + val, 0);
    totalSumEl.innerHTML = totalSum.toFixed(2);
    differenceEl.innerHTML = (income - totalSum).toFixed(2);
}