function improveCodeBlocksColouring() {
    const elements = [...document.getElementsByClassName("p")];
    const openingParenthesesElements = elements.filter(elt => elt.textContent.startsWith("("));

    openingParenthesesElements.forEach(element => {
        const elementBeforeParenthese = element.previousElementSibling;
        if (elementBeforeParenthese != null) {
            let elementBeforeClassOfFunction = elementBeforeParenthese.previousElementSibling;

            if (elementBeforeClassOfFunction == null || elementBeforeClassOfFunction.textContent === "new") {
                elementBeforeParenthese.classList.add("class");
            } else {
                elementBeforeParenthese.classList.add("function");
            }
        }
    });

    const genericsOpeningParenthesesElements = elements.filter(elt => elt.textContent.endsWith(">("));
    genericsOpeningParenthesesElements.forEach(elementBeforeClassOfFunction => {
        while (elementBeforeClassOfFunction != null && elementBeforeClassOfFunction.textContent !== "<") {
            elementBeforeClassOfFunction = elementBeforeClassOfFunction.previousElementSibling;
        }

        if (elementBeforeClassOfFunction == null) {
            return;
        }

        elementBeforeClassOfFunction = elementBeforeClassOfFunction.previousElementSibling;
        if (elementBeforeClassOfFunction != null) {
            elementBeforeClassOfFunction.classList.add("function");
        }
    })
}

function improveIteratorCodeBlocks() {
    const iterableCodeBlocks = [...document.querySelectorAll(".iterator-available .highlight")];
    iterableCodeBlocks.forEach(element => {
        element.insertAdjacentHTML("afterbegin", '<a class="iterator" href="../twitter-api/iterators.html"><b>iterators</b> <span class="fa fa-arrow-circle-right" style="margin-left: 10px;"></span></a>');
    });
}

function addGithubLink() {
    const navBar = document.getElementsByClassName("wy-nav-side")[0];
    const text = document.createElement("div");
    text.innerHTML = `
    <a href="https://github.com/linvi/tweetinvi"><svg height="128" class="octicon octicon-mark-github" viewBox="0 0 16 16" version="1.1" width="128" aria-hidden="true"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0016 8c0-4.42-3.58-8-8-8z"></path></svg></a>
    <span style="position: absolute; top: 6px; left: 80px;">Version 5.0-alpha-8</span>`;
    navBar.parentNode.insertBefore(text, navBar.nextSibling)
    text.classList.add("navbar-icons");
}

document.addEventListener("DOMContentLoaded", function (event) {
    improveCodeBlocksColouring();
    improveIteratorCodeBlocks();
    addGithubLink();
});