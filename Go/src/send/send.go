package main

import (
	"fmt"
	"log"

	"github.com/michaelbironneau/asbclient"
)

func main() {

	for i := 0; i <= 1000; i++ {
		namespace := "bialecki"
		keyname := "RootManageSharedAccessKey"
		keyvalue := "39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY="

		client := asbclient.New(asbclient.Topic, namespace, keyname, keyvalue)

		err := client.Send("go_testing", &asbclient.Message{
			Body: []byte(fmt.Sprintf("message %d", i)),
		})

		if err != nil {
			log.Printf("Send error: %s", err)
		} else {
			log.Printf("Sent: %d", i)
		}
	}

	log.Printf("Done")
}
