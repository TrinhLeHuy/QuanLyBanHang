
let urlCreate = document.getElementById("orderCreate").value;
let urlDetail = document.getElementById("orderDetail").value;
let urlTable = document.getElementById("orderTable").value;
let urlFilter = document.getElementById("orderFilter").value;

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("btn-remove")) {
        e.target.closest("tr").remove();
    }
});
//----------------Filter-------------------
document.getElementById("btnFilterOrder").addEventListener("click", function (e) {
    e.preventDefault();
    const stringfilter = document.getElementById("strFilterOrder").value.trim();
    reloadFilter(stringfilter);
    document.getElementById("btnRefreshOrder").style.display = "block";
});
document.getElementById("btnRefreshOrder").addEventListener("click", function (e) {
    e.preventDefault();
    reloadTable();
    document.getElementById("strFilterOrder").value="";
    this.style.display = "none";
});
//----------------Create-------------------

document.getElementById("btnCreateOrder").addEventListener("click", function () {

    fetch(urlCreate)
        .then(response => response.text())
        .then(html => {
            let modal = document.getElementById("create-container");
            let content = document.getElementById("create-content");

            content.innerHTML = html;
            modal.style.display = "flex";
            initOrderCreate(content);
            eventOrderCreate(content);
            content.querySelector("#addRow").click();
            const voucher = content.querySelector(`#selectVoucher`);
            const totaAmount = content.querySelector(`#TotalAmount`);
            voucher.addEventListener("change", function () {
                var discount = Number(voucher.options[voucher.selectedIndex].getAttribute("data-discountvalue"));
                if (discount < 1.0) {
                    totaAmount.innerText = (Math.floor(totaAmount.dataset.total) * (1.0 - discount)).toString();
                } else if (discount > 0) {
                    totaAmount.innerText = (Math.floor(totaAmount.dataset.total) - Math.floor(discount)).toString();
                } else {
                    totaAmount.innerText = (totaAmount.dataset.total).toString();
                }
            })
        })
        .catch(err => console.error("Lỗi load Create:", err));
});

document.getElementById("create-container").addEventListener("click", function (e) {
    if (e.target === this) {
        this.style.display = "none";
        document.getElementById("create-content").innerHTML = "";
    }
});

//----------------Detail-------------------

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("btn-info")) {
        const id = e.target.dataset.id;

        console.log("Xem chi tiết:", id);
        fetch(`${urlDetail}?orderId=${id}`)
            .then(response => response.text())
            .then(html => {
                let modal = document.getElementById("detail-container");
                let content = document.getElementById("detail-content");
                content.innerHTML = html;
                modal.style.display = "flex";
                eventOrderUpdate(content);
            })
            .catch(err => console.error("Lỗi load Detail:", err));
    }
});


document.getElementById("detail-container").addEventListener("click", function (e) {
    if (e.target === this) {
        this.style.display = "none";
        document.getElementById("detail-content").innerHTML = "";
    }
});

//-----------------------------------

function initOrderCreate(content) {
    let index = parseInt(content.querySelector("#orderDetailIndex").value);
    let productOptions = content.querySelector("#productOptionsHtml").textContent;

    content.querySelector("#addRow").addEventListener("click", function () {
        let table = content.querySelector("#tableDetailOrders tbody");

        let row = `
                <tr id="row${index}" data-sum=0>
                    <td>
                        <select name="OrderDetails[${index}].ProductId" class="form-select" id="select${index}">
                            ${productOptions}
                        </select>
                    </td>
                    <td>
                        <input name="OrderDetails[${index}].Quantity" class="form-control quantity" type="text" value="1" id="quantity${index}"/>
                    </td>
                    <td>
                        <input name="OrderDetails[${index}].UnitPrice" class="form-control  price" type="number" value="0" id="price${index}" readonly />
                    </td>
                    <td><button type="button" class="btn btn-danger btn-remove" id="remove${index}">X</button></td>
                </tr>
    `;
        table.insertAdjacentHTML("beforeend", row);
        const rowselet = content.querySelector(`#row${index}`);
        const select = content.querySelector(`#select${index}`);
        const price = content.querySelector(`#price${index}`);
        const quantity = content.querySelector(`#quantity${index}`);
        const defaultprice = Number(select.options[0].getAttribute("data-price"));
        const totaAmount = content.querySelector(`#TotalAmount`);
        const btnremove = content.querySelector(`#remove${index}`);
        const voucher = content.querySelector(`#selectVoucher`);
        var discountdefault = Number(voucher.options[voucher.selectedIndex].getAttribute("data-discountvalue"));
        price.value = Math.floor(defaultprice * 1.1);
        rowselet.dataset.sum = Math.floor(price.value);
        totaAmount.dataset.total = Math.floor(totaAmount.dataset.total) + Math.floor(rowselet.dataset.sum);
        if (discountdefault < 1) {
            totaAmount.innerText = (Math.floor(totaAmount.dataset.total) * (1.0 - discountdefault)).toString();
        } else if (discountdefault > 0) {
            totaAmount.innerText = (Math.floor(totaAmount.dataset.total) - Math.floor(discountdefault)).toString();
        } else {
            totaAmount.innerText = (totaAmount.dataset.total).toString();
        }
        
        select.addEventListener("change", function () {
            const dataprice = Number(select.options[select.selectedIndex].getAttribute("data-price"));
            price.value = Math.floor(dataprice * 1.1);
            var temp1 = Math.floor(rowselet.dataset.sum);
            const num = parseInt(quantity.value);
            rowselet.dataset.sum = Math.floor(price.value * num);
            var discount = Number(voucher.options[voucher.selectedIndex].getAttribute("data-discountvalue"));
            totaAmount.dataset.total = Math.floor(totaAmount.dataset.total) - temp1 + Math.floor(rowselet.dataset.sum);
            if (discount < 1) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) * (1.0 - discount)).toString();
            } else if (discountt > 0) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) - Math.floor(discount)).toString();
            } else {
                totaAmount.innerText = (totaAmount.dataset.total).toString();
            }
        })
        quantity.addEventListener("change", function () {
            this.value = this.value.replace(/[^0-9]/g, '');
            var temp = this.value;
            var discount = Number(voucher.options[voucher.selectedIndex].getAttribute("data-discountvalue"));
            if (temp != "") {
                var temp1 = Math.floor(rowselet.dataset.sum);
                const num = parseInt(this.value);
                rowselet.dataset.sum = Math.floor(price.value * num);
                totaAmount.dataset.total = Math.floor(totaAmount.dataset.total) - temp1 + Math.floor(rowselet.dataset.sum);
            }
            if (discount < 1) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) * (1.0 - discount)).toString();
            } else if (discount > 0) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) - Math.floor(discount)).toString();
            } else {
                totaAmount.innerText = (totaAmount.dataset.total).toString();
            }
        })
        index++;
        btnremove.addEventListener("click", function () {
            var discount = Number(voucher.options[voucher.selectedIndex].getAttribute("data-discountvalue"));
            totaAmount.dataset.total = Math.floor(totaAmount.dataset.total) - Math.floor(rowselet.dataset.sum);
            if (discount < 1) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) * (1.0 - discount)).toString();
            } else if (discount > 0) {
                totaAmount.innerText = (Math.floor(totaAmount.dataset.total) - Math.floor(discount)).toString();
            } else {
                totaAmount.innerText = (totaAmount.dataset.total).toString();
            }
        });
    });

}

function eventOrderCreate(content) {
    const form = content.querySelector("#orderCreateForm");
    form.addEventListener("submit", function (e) {
        e.preventDefault();
        const formData = new FormData(form);
        fetch(form.action, { method: "POST", body: formData })
            .then(resp => resp.text())
            .then(html => {
                if (html.includes('<form')) {
                    content.innerHTML = html;
                    eventOrderCreate(content);
                } else {
                    document.getElementById("create-container").style.display = "none";
                    reloadTable();
                }
            })
            .catch(err => console.error(err));
    });
}

function eventOrderUpdate(content) {
    const form = content.querySelector("#orderUpdateForm");
    form.addEventListener("submit", function (e) {
        e.preventDefault();
        const formData = new FormData(form);
        fetch(form.action, { method: "POST", body: formData })
            .then(resp => resp.text())
            .then(html => {
                if (html.includes('<form')) {
                    content.innerHTML = html;
                    eventOrderUpdate(content)
                } else if (html.includes('404')) {
                    alert.log("404");
                }
                else {
                    document.getElementById("detail-container").style.display = "none";
                    reloadTable();
                }
            })
            .catch(err => console.error(err));
    });
}


function reloadTable() {
    fetch(urlTable)
        .then(res => res.text())
        .then(html => {
            document.querySelector("#orders-table").innerHTML = html;
        })
        .catch(err => console.error("Lỗi reloadTable:", err));
}

function reloadFilter(stringfilter) {
    fetch(`${urlFilter}?filter=${stringfilter}`)
        .then(res => res.text())
        .then(html => {
            document.querySelector("#orders-table").innerHTML = html;
        })
        .catch(err => console.error("Lỗi reloadFilter:", err));
}