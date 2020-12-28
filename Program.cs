using System;
using RPC;
using System.Collections.Generic;
using console.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace console
{
    class Program
    {
        static RpcClient rpcClient = new RpcClient(); 

        static void Main(string[] args)
        {
            string input = "";
            string method, action = "";

            while (input != "stop") {
                input = Console.ReadLine();
                var split = input.Split(":");
                if (split.Length > 1) {
                    switch(split[0]) {
                        case "insert":
                            insertTodoItem(split[1]);
                            break;
                        case "get":
                            getTodoItem(split[1]);
                            break;
                        case "getAll":
                            getAllTodoItems();
                            break;
                        default:
                            break;
                    }
                }
            }
            

            rpcClient.Close();
            
        }

        static void insertTodoItem(string todo) {
            var send = new TodoItem() {
                Name = todo,
                IsComplete = false
            };
            var json = JsonConvert.SerializeObject(send);
            Console.WriteLine(json);
            var response = rpcClient.Call("rpc_insertTodoItem", json);
            Console.WriteLine(response);
            // response = JsonConvert.DeserializeObject()
            var newTodo = JsonConvert.DeserializeObject<TodoItem>(response);
            Console.WriteLine($"received '{newTodo.Name}' from server");
        }

        static void getTodoItem(string id) {
            Console.WriteLine($"Get id: {id}");
            // var json = JsonConvert.SerializeObject(id);
            // Console.WriteLine($"json: {json}");
            var response = rpcClient.Call("rpc_getTodoItem", id);
            Console.WriteLine(response);
            var todo = JsonConvert.DeserializeObject<TodoItem>(response);
            if (todo != null) {
                Console.WriteLine($"received '{todo.Name}' from server");
            }
        }

        static void getAllTodoItems() {
            var response = rpcClient.Call("rpc_getAllTodoItems", "");
            Console.WriteLine(response);
            var items = JsonConvert.DeserializeObject<List<TodoItem>>(response);

        }
    }
}
