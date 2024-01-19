export function scrollToEnd() {
  var MessageContainer = document.getElementById("mainchat");
  MessageContainer.scrollTop = MessageContainer.scrollHeight;
}

export function incrementFollowingCount() {
  var countElement = document.getElementById("followingCount");
  var count = parseInt(countElement.innerText);
  countElement.innerText = count + 1;
}

export function decrementFollowingCount() {
  var countElement = document.getElementById("followingCount");
  var count = parseInt(countElement.innerText);
  countElement.innerText = count - 1;
}
