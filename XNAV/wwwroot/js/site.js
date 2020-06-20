// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var global = {};

async function graphql(equipment, tags) {
    let url = "/graphql";
    let eq;
    if (equipment.includes("tags")) {
        eq = equipment.join(" ");
        eq=eq.replace("tags", `tags{ ${tags.join(" ")} }`);
        console.log(eq);
    }
    else {
        eq = equipment.join(" ");
    }

    document.getElementById('graphql-container').innerHTML = '';
    fetch('/', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify({ query: `{ equipment{ id ${eq} }}` })
    })
        .then(r => r.json())
        .then(response => {
            console.log(response);
            for(const row of response.data.equipment)
            {
                addDivToContainer(document.getElementById("graphql-container"), row);
            }
        });
}

function addDivToContainer(wrapper, info) {
    var row = document.createElement('div'); setAttributes(row, { "onclick": "clicked(this);", "style": "display:flex;flex-wrap:wrap;justify-content:space-around;", "class": "text-wrapper", "id": "gear-" + info.id })
    for (let [key, value] of Object.entries(info)) {
        switch (key) {
            case 'description':
                addInformationAsChild('div', { "style": "flex-grow:4;width:400px;font-weight:bold;" }, value, row);
                break;
            case 'id':
                break;
            case 'tags':
                if (value.length !== 0) {
                    let str = '';
                    for (const l of value) {
                        for (const k of global.tags) {
                            str += " " + l[k];
                        }
                        str += ", ";
                    }
                    addInformationAsChild('div', { "style": "flex-grow:1;width:200px;font-weight:bold;" }, str, row);
                }
                else {
                    addInformationAsChild('div', { "style": "flex-grow:1;width:200px;font-weight:bold;" }, "No Tags!", row);
                }
                break;
            default:
                addInformationAsChild('div', { "style": "flex-grow:1;width:200px;font-weight:bold;" }, value, row);
                break;
        }
    }
    wrapper.appendChild(row);
}



function setAttributes(el, attrs) {         //function for setting multiple attributes on an element at once
    for (var key in attrs) {
        el.setAttribute(key, attrs[key]);
    }
}

function addInformationAsChild(type = 'div', attrs = {}, textNode = "", parent) {
    var item = document.createElement(type); setAttributes(item, attrs);
    if (textNode) { var tmpTextNode = document.createTextNode(textNode); item.appendChild(tmpTextNode); }
    parent.appendChild(item);
}

function clicked(el) { //TODO finish this up
    console.log(el);
}

$("#graphql-selectpicker").on("changed.bs.select", e => {
    global.options = $("#graphql-selectpicker").val();
    if (global.options.includes('tags')) {
        $("#tags-selectpicker").selectpicker('show');
    }
    else {
        $("#tags-selectpicker").selectpicker('hide');
    }
});

$("#tags-selectpicker").on("changed.bs.select", e => {
    global.tags = $("#tags-selectpicker").val();
})

$("#submit").on("click", e=> {
    graphql(global.options, global.tags);
})

$(() => {
    $("#graphql-selectpicker").selectpicker('val', ['manufacturer', 'model', 'description']);
    global.options = ['manufacturer', 'model', 'description'];
    $("#tags-selectpicker").selectpicker('val', ['tag']);
    global.tags = ['tag'];
    $("#tags-selectpicker").selectpicker('hide');
    graphql(global.options, global.tags);
})

