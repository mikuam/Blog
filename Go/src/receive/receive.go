package main

import (
	"log"

	"github.com/mikuam/asbclient"
)

func main() {

	namespace := "bialecki"
	keyname := "RootManageSharedAccessKey"
	keyvalue := "39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY="

	client := asbclient.New(asbclient.Queue, namespace, keyname, keyvalue)
	log.Printf("Peeking...")

	for {
		msg, err := client.PeekLockMessage("go_testing", 30)

		if err != nil {
			log.Printf("Peek error: %s", err)
		} else {
			log.Printf("Peeked message: '%s'", string(msg.Body))
			err = client.DeleteMessage(msg)
			if err != nil {
				log.Printf("Delete error: %s", err)
			}
		}
	}
}
