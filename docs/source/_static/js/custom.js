function improveCodeBlocksColouring() {
    const elements = [...document.getElementsByClassName("p")];
    const openingParenthesesElements = elements.filter(elt => elt.textContent.startsWith("("));
    openingParenthesesElements.forEach(element => {
        const elementBeforeParenthese = element.previousElementSibling;
        if (elementBeforeParenthese != null) {
            const elementBeforeClassOfFunction = elementBeforeParenthese.previousElementSibling;

            if (elementBeforeClassOfFunction == null || elementBeforeClassOfFunction.textContent === "new") {
                elementBeforeParenthese.classList.add("class");
            } else {
                elementBeforeParenthese.classList.add("function");
            }
        }
    });
}

function improveIteratorCodeBlocks() {
    const iterableCodeBlocks = [...document.querySelectorAll(".iterator-available .highlight")];
    iterableCodeBlocks.forEach(element => {
        console.log(element);
        element.insertAdjacentHTML("afterbegin", '<a class="iterator" href="/features/iterators.html"><b>iterators</b> <span class="fa fa-arrow-circle-right" style="margin-left: 10px;"></span></a>');
    });
}

document.addEventListener("DOMContentLoaded", function (event) {
    improveCodeBlocksColouring();
    improveIteratorCodeBlocks();
});