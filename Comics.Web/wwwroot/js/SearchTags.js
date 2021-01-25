var tagsModal = document.getElementById("tagsModal"); //when creating object
var gameCardTagInputs = document.getElementsByName("ItemTagCardInput"); //when displaying cards of objects
var gameCards;
gameCardTagInputs.forEach(e => { //for each card or for each tag in each card?
    var tag = new Tagify(e);
})
initTagsInput();
initTagsModal();

function initTagsInput() {
    var tagsInput = document.getElementById("tagsInput"); //search input in the top of _Layout
    var tagifyInput = initTags(tagsInput);
    tagifyInput.on('add', e => {
        enforceMaxChars(tagifyInput);
        filter();
    });
    tagifyInput.on('remove', filter);
}
function initTagsModal() {
    var tagifyModal = initTags(tagsModal);
    tagifyModal.on('add', e => enforceMaxChars(tagifyModal));
}
function initTags(tagsElement) {
    var allTags = getAllActiveTags();
    var tagifyElement = new Tagify(tagsElement, {
        whitelist: allTags,
        maxTags: 3,
        dropdown: {
            maxItems: 20,
            classname: "tags-look",
            enabled: 0,
            closeOnSelect: false
        }
    })
    return tagifyElement
}

function getAllActiveTags() {
    var allTags = [];
    gameCardTagInputs.forEach(e => {
        var gameCardTags;
        if (e.value == "") gameCardTags = e.value; else {
            gameCardTags = JSON.parse(e.value);
        }
        for (var i = 0; i < gameCardTags.length; i++) {
            if (isTagUnique(allTags, gameCardTags[i]))
                allTags.push(gameCardTags[i]);
        }
    })
    return allTags;
}

function isTagUnique(allTags, gameCardTag) {
    var isIncluded = false;
    allTags.forEach(el => {
        if (el.value == gameCardTag) isIncluded = true;
    })
    return !isIncluded;
}
function enforceMaxChars(tagify) {
    if (tagify.DOM.input.textContent.length > 11)
        tagify.removeTag();
}

function filter() {
    gameCards = gameCards = document.getElementsByName("ItemCard");
    removeDisplayNone();
    filterByTags();
    filterByJoinable();
}
function removeDisplayNone() {
    gameCards.forEach(e => {
        if (e.classList.contains("d-none"))
            e.classList.remove("d-none");
    })
}

function filterByTags() {
    var tags = getTags();
    if (tags.length == 0) {
            return;
        }
    gameCards.forEach(el => {
        var tagifiers = el.getElementsByClassName("tagify__tag");
        var hasAllFilterTags = true;
            for (var i = 0; i < tags.length; i++) {
                for (var j = 0; j < tagifiers.length; j++) {
                    hasAllFilterTags = true;
                    if (tagifiers[j].attributes["title"].value == tags[i]) break;
                    hasAllFilterTags = false;
                }
                if (!hasAllFilterTags) break;
            }
            if (!hasAllFilterTags) addNoDisplay(el);
        })
}

function getTags() {
    var tagsInputDiv = document.getElementById("tagsInputDiv");
    var filtertags = tagsInputDiv.getElementsByClassName("tagify__tag");
    var tags = [];
    for (var i = 0; i < filtertags.length; i++) {
        tags.push(filtertags[i].attributes["title"].value);
    }
    return tags;
}

function addNoDisplay(element) {
    if (element.classList.contains("d-none")) return;
    element.classList.add("d-none");
}