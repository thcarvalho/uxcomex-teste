getData();

$(() => {
  $("#addClientButton").on("click", (event) => {
    event.preventDefault();
    const name = $("#nameInput").val();
    const email = $("#emailInput").val();
    const phone = $("#phoneInput").val();

    $.ajax({
      url: "https://localhost:7150/api/clients",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({ name, email, phone }),
      success: () => {
        $("#addClientModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao adicionar cliente!");
      },
    });
  });
});

$(document).on("click", "#editClientModalButton", element => {
  const clientId = $(element.currentTarget).val();
  $("#editClientButton").data("clientId", clientId);

  $.get(`https://localhost:7150/api/clients/${clientId}`, (data) => {
    $("#clientIdEditInput").val(clientId);
    $("#nameEditInput").val(data.name);
    $("#emailEditInput").val(data.email);
    $("#phoneEditInput").val(data.phone);

    $("#editClientModal").modal("show");
  });
});

$(document).on("click", "#deleteClientModalButton", element => {
  const clientId = $(element.currentTarget).val();
  $("#deleteClientButton").data("clientId", clientId);
  $("#deleteClientModal").modal("show");
});

$(() => {
  $("#editClientButton").on("click", (event) => {
    event.preventDefault();
    const id = $("#clientIdEditInput").val();
    const name = $("#nameEditInput").val();
    const email = $("#emailEditInput").val();
    const phone = $("#phoneEditInput").val();

    $.ajax({
      url: `https://localhost:7150/api/clients/${id}`,
      type: "PUT",
      contentType: "application/json",
      data: JSON.stringify({ name, email, phone }),
      success: () => {
        $("#editClientModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao editar cliente!");
      },
    });
  });
});

$(() => {
  $("#deleteClientButton").on("click", (event) => {
    event.preventDefault();
    const id = $("#deleteClientButton").data("clientId");
    $.ajax({
      url: `https://localhost:7150/api/clients/${id}`,
      type: "DELETE",
      contentType: "application/json",
      success: () => {
        $("#deleteClientModal").modal("hide");
        location.reload();
      },
      error: () => {
        alert("Erro ao deletar cliente!");
      },
    });
  });
});

$(() => {
  $("#searchButton").on("click", (event) => {
    event.preventDefault();
    const filter = $("#filterSelect").val();
    const search = $("#searchInput").val();
    getData(search, filter);
  });
});

function getData(search = "", field = 0, page = 1, itemsPerPage = 10) {
  let url = `https://localhost:7150/api/clients?pageNumber=${page}&itemsPerPage=${itemsPerPage}`;
  if (search !== undefined) {
    url += `&search=${search}&field=${field}`;
  }
  $.get(url, (data) => {
    const tbody = $("table tbody");
    tbody.empty();
    data.items.forEach((client) => {
      const row = `<tr>
            <td hidden class="clientId">${client.id}</td>
            <td>${client.name}</td>
            <td>${client.email}</td>
            <td>${client.phone}</td>
            <td>${new Date(client.registerDate).toLocaleDateString()}</td>
            <td>
            <div class="btn-group" role="group">
                <button type="button" class="btn btn-outline-primary">
                <a class="link-offset-2 link-underline link-underline-opacity-0" href="./new-order.html?clientId=${
                  client.id
                }"><i class="bi bi-bag"></i></a>
                </button>
                <button id="editClientModalButton" value="${client.id}" type="button" class="btn btn-outline-primary">
                <i class="bi bi-pencil"></i>
                </button>
                <button id="deleteClientModalButton" value="${client.id}"  type="button" class="btn btn-outline-primary">
                <i class="bi bi-trash"></i>
                </button>
            </div>
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
    console.error("Erro ao pesquisar clientes!");
  });
}
