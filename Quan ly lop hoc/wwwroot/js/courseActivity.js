// Assuming courseId is known
var courseId = document.getElementById('form-input-course-id').value;
var listActivitiesContainer = document.getElementById('list-activities');
var url_assignment = `/course/${courseId}/activities?orderBy=startDate&sortBy=desc`;
// var url_meeting = `/course/${courseId}/meetings?orderBy=startDate&sortBy=desc`;
// Create a new XMLHttpRequest object
var xhr = new XMLHttpRequest();

// Configure the request
xhr.open('GET',url_assignment, true);
xhr.setRequestHeader('Content-Type', 'application/json');

// Set up a callback function to handle the response
xhr.onload = function() {
    if (xhr.status >= 200 && xhr.status < 300) {
        // Request was successful
        var data = JSON.parse(xhr.responseText);
        data.forEach(function(activity) {
            var activityDiv = document.createElement('div');
            activityDiv.classList.add('activity');
            var iconElement = document.createElement('i');
            var nameLink = document.createElement('a');
            iconElement.classList.add('fa-regular', 'fa-file');
            if (activity.url != null){
                nameLink.href = `/meeting/${activity.id}`;
            } else {
                nameLink.href = `/assignment/${activity.id}`;
            }
            var typeSpan = document.createElement('span');
            typeSpan.textContent = activity.name;
            activityDiv.appendChild(nameLink);
            nameLink.appendChild(iconElement);
            nameLink.appendChild(typeSpan);
            nameLink.classList.add('btn', 'btn-outline', 'btn-outline-secondary', 'w-100', 'm-4', 'p-4');
            // Append the activity div to the container
            listActivitiesContainer.appendChild(activityDiv);
      });
    } else {
        // Request failed
        console.error('Request failed with status:', xhr.status);
    }
};

// Set up a callback function to handle errors
xhr.onerror = function() {
    // Handle any errors that occur during the request
    console.error('Request failed');
};

// Send the request
xhr.send();