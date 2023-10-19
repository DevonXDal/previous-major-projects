import { Fragment, useEffect, useState } from "react";
import { fetchApiMessageFeed } from "../../utils/api-handlers/api-message-feed.util";

const TEN_SECONDS = 10000;

/* Message:
    title: string,
    content: string,
    created: Date
*/

const MessageFeed = () => {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        const messageFeedUpdateInterval = setInterval(async () => {
            const potentialMessages = await fetchApiMessageFeed();
            let fetchedMessages = null;

            if (potentialMessages) {
                fetchedMessages = potentialMessages.Messages;
            }

            if (fetchedMessages && messages !== fetchedMessages) {
                setMessages(fetchedMessages);
            }
        }, TEN_SECONDS); 

        return () => clearInterval(messageFeedUpdateInterval); // Clear on destroy
    }, [messages]);

    return (
        <Fragment>
            <h2>Message Feed  <i className="bi bi-chat-left-dots"></i></h2>
            <h5>Checks every 10 seconds</h5>

            <div className="row my-1" >
            {
                messages.map(message => {
                    return (
                    <div className="col-12 card my-1">
                        <div className="card-header">
                            {message.title}
                        </div>
                        <div className="card-body">
                            {message.content}
                        </div>
                        <div className="card-footer">
                            Created: {message.created}
                        </div>
                    </div>
                    );
                })
            }
            </div>
            
        </Fragment>
    );
}

export default MessageFeed;