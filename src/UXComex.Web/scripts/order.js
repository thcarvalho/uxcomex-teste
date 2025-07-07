getData();
const statusOptions = [
  { value: 0, text: "Pendente" },
  { value: 1, text: "Processando"},
  { value: 2, text: "Enviado"},
  { value: 3, text: "Concluido"},
  { value: 4, text: "Cancelado"},
];


function getData(search = "", field = 0, page = 1, itemsPerPage = 10) {
  let url = `https://localhost:7150/api/orders?pageNumber=${page}&itemsPerPage=${itemsPerPage}`;
  if (search !== undefined) {
    url += `&search=${search}&field=${field}`;
  }

  $.get(url, (data) => {
    const tbody = $("table tbody");
    tbody.empty();
    data.items.forEach((order) => {
      const row = `<tr id="detailsModalButton" onmouseover="this.style.cursor='pointer'">
            <td hidden class="orderId">${order.id}</td>
            <td>${order.clientName}</td>
            <td>${new Date(order.orderDate).toLocaleDateString()}</td>
            <td>R$ ${order.totalAmount.toFixed(2).replace(".", ",")}</td>
            <td>${statusOptions.find(x => x.value === order.status).text}</td>
            <td>
            <td>
              <button
                  value="${order.id}"
                  id="changeStatusModalButton"
                  type="button"
                  class="btn btn-outline-primary">
                  <i class="bi bi-pencil"></i>
              </button>
            </td>
        </tr>`;
      tbody.append(row);
    });

    const pagination = $("#paginationNav");
    pagination.empty();

    const totalPages = data.totalPages;
    const currentPage = data.page;

    const previous = data.page > 1 
      ? `<li class="page-item"><a class="page-link" href="#" onclick="getData('${search}', '${field}', ${data.page - 1}, ${itemsPerPage})">Anterior</a></li>`
      : `<li class="page-item disabled"><a class="page-link">Anterior</a></li>`

    const next = data.page === totalPages
      ? `<li class="page-item disabled"><a class="page-link">Próxima</a></li>`
      : `<li class="page-item"><a class="page-link" href="#" onclick="getData('${search}', '${field}', ${data.page + 1}, ${itemsPerPage})">Próxima</a></li>`

    let pages = ""

    for (let index = 1; index <= totalPages; index++) {
      pages += 
       `<li class="page-item ${index === currentPage ? 'active' : ''}">
          <a class="page-link" href="#" onclick="getData('${search}', '${field}', ${index}, ${itemsPerPage})">${index}</a>
        </li>`            
    }  
    const pagRows = 
    `
      <ul class="pagination">
        ${previous}
        ${pages}
        ${next}
      </ul>
    `
    pagination.append(pagRows);
  }).fail(() => {
    console.error("Erro ao pesquisar pedidos!");
  });
}

$(document).on("click", "#detailsModalButton", (element) => {
  if (element.target.tagName === "BUTTON") return;

  const orderId = $(element.currentTarget).find(".orderId").text();
  $.get(`https://localhost:7150/api/orders/${orderId}`, (data) => {
    $("#clientNameLabel").text(data.clientName);
    $("#dateLabel").text(new Date(data.orderDate).toLocaleDateString());
    $("#totalLabel").text(data.totalAmount.toFixed(2).replace(".", ","));
    $("#statusLabel").text(statusOptions.find(x => x.value === data.status).text);

    const list = $("#itemList");
    list.empty();
    data.orderItems.forEach((item) => {
      const row = `<li>${item.quantity}x - ${item.productName} - R$ ${item.unitPrice}</li>`;
      list.append(row);
    });

    $("#detailsModal").modal("show");
  });
});

$(document).on("click", "#changeStatusModalButton", (element) => {
  const orderId = $(element.currentTarget).val();
  $("#changeStatusButton").data("orderId", orderId);
  $("#changeStatusModal").modal("show");
});

$(() => {
  $("#changeStatusButton").on("click", (event) => {
    event.preventDefault();
    const id = $("#changeStatusButton").data("orderId");
    const status = $("#orderStatusSelect").val();

    $.ajax({
      url: `https://localhost:7150/api/orders/status/${id}`,
      type: "PATCH",
      contentType: "application/json",
      data: status,
      success: () => {
        $("#changeStatusModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao alterar status!");
      },
    });
  });
});

$(document).on("click", "#filterSelect", (element) => {
  element.preventDefault();
  if ($(element.target).val() === "0") {
    $("#searchInput").empty();
    $.get(`https://localhost:7150/api/clients`, (data) => {
      $("#searchInput").append(
        '<option value="" selected>Selecione um cliente</option>'
      );
      data.items.forEach((client) => {
        const option = `<option value="${client.id}">${client.name}</option>`;
        $("#searchInput").append(option);
      });
    });
  } else if ($(element.target).val() === "1") {
    $("#searchInput").empty();
    $("#searchInput").append(
      `<option value="" selected>Selecione um status</option>
         <option value="0">Pendente</option>
         <option value="1">Processando</option>
         <option value="2">Enviado</option>
         <option value="3">Concluido</option>
         <option value="4">Cancelado</option>`
    );
  }
});

$(document).on("click", "#searchButton", (element) => {
  element.preventDefault();
  const filter = $("#filterSelect").val();
  const search = $("#searchInput").val();
  getData(search, filter);
});
